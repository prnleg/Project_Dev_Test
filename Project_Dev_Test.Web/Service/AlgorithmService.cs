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

        public AlgorithmService(DataRepository repository)
        {
            this.repository = repository;
        }

        public ResultObject GetResult(Vector<double> g, AlgorithmEnum algorithm)
        {
            ProcessingMetrics metrics = new ProcessingMetrics();

            // Process image
            var start = DateTime.Now;
            metrics.StartProcessing();

            (Vector<double> result, uint iterations) processed =
                    algorithm == AlgorithmEnum.CGNE ? CGNESolver.Solve(g) : CGNRSolver.Solve(g);

            var end = DateTime.Now;
            metrics.EndProcessing();

            var processedImage = processed.result;
            var iterations = processed.iterations;

            if (processedImage.Count != 60 * 60)
                throw new Exception("Processed image size != 60x60");

            double[,] imageArray = new double[60, 60];

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    imageArray[j, i] = processedImage[i * 60 + j];
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
