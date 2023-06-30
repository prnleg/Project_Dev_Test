using MathNet.Numerics.LinearAlgebra;
using Project_Dev_Test.Core.Handlers;
using Project_Dev_Test.Web.Algorithm;
using Project_Dev_Test.Web.Models;
using Project_Dev_Test.Web.Repository;
using System.Drawing;

namespace Project_Dev_Test.Web.Service
{
    public class AlgorithmService
    {
        private readonly DataRepository repository;
        private readonly ProcessQueueService processQueue;

        public AlgorithmService(DataRepository repository, ProcessQueueService processQueue)
        {
            this.repository = repository;
            this.processQueue = processQueue;
        }

        public async Task<ResultObject> GetResult(Vector<double> g, AlgorithmEnum algorithm)
        {
            ProcessingMetrics metrics = new ProcessingMetrics();
            int gSize = g.Count() == 50816 ? 60 : 30;

            // Process image
            var start = DateTime.Now;
            metrics.StartProcessing();

            var task = new Task<(Vector<double> result, uint iterations)>(() =>
            {
                return algorithm == AlgorithmEnum.CGNE ? CGNESolver.Solve(g, gSize) : CGNRSolver.Solve(g, gSize);
            });

            await processQueue.Enqueue(task);

            // Get results
            Vector<double> processedImage = null;
            uint iterations = 0;

            await task.ContinueWith((task) =>
            {
                processedImage = task.Result.result;
                iterations = task.Result.iterations;
            });

            var end = DateTime.Now;
            metrics.EndProcessing();

            if (processedImage.Count != gSize * gSize)
                throw new Exception("Processed image size != 60x60");

            double[,] imageArray = new double[gSize, gSize];

            for (int i = 0; i < gSize; i++)
            {
                for (int j = 0; j < gSize; j++)
                {
                    imageArray[j, i] = processedImage[i * gSize + j];
                }
            }

            byte[] imgReturn = null;

            Bitmap bitmap;
            unsafe
            {
                fixed (double* intPtr = &imageArray[0, 0])
                {
                    ImageConverter converter = new ImageConverter();
                    imgReturn = converter.ConvertTo(Helpers.ToBitmap(imageArray), typeof(byte[])) as byte[];
                }
            }

            float cpuUsage = metrics.GetCpuUsage();
            float ramUsage = metrics.GetRamUsageMB();
            TimeSpan elapsedTime = metrics.GetElapsedTime();

            Console.WriteLine("CPU Usage: {0}%", cpuUsage);
            Console.WriteLine("RAM Usage: {0} MB", ramUsage);
            Console.WriteLine("Elapsed Time: {0}", elapsedTime);

            ResultObject resultObject = new ResultObject()
            {
                Id = Guid.NewGuid(),
                Image = Convert.ToBase64String(imgReturn),
                CPU = cpuUsage,
                Memory = ramUsage,
                Iterations = iterations,
                TimeElapsed = elapsedTime.TotalMilliseconds,
                StartOperation = start,
                EndOperation = end
            };

            return resultObject;
        }

        public void SaveResult(ResultObject result, int userId)
        {
            result.User = userId;

            try
            {
                repository.SaveResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AlgorithmService - Error saving Result Object: " + ex.ToString());
            }
        }

    }
}
