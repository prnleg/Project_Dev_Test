using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Handlers;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;
using Project_Dev_Test.Web.Models;
using Project_Dev_Test.Web.Readers;
using System.Drawing;
using System.Drawing.Imaging;

namespace Project_Dev_Test.Web.Api
{
    public class CGNRController : Controller
    {
        private readonly IRepository _repository;

        public CGNRController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("{userId}/CGNR-SolverImageSignal")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CGNRImageSignal([FromRoute] int userId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            ProcessingMetrics metrics = new ProcessingMetrics();

            MemoryStream mstream = new MemoryStream();
            await HttpContext.Request.Body.CopyToAsync(mstream);

            var formFile = new FormFile(mstream, 0, mstream.Length, "image-csv", "image-csv");

            if (formFile == null)
            {
                throw new ArgumentNullException("Image CSV is null or empty");
            }

            var file = CSVFileReader.CSVFileReaderVector(formFile);
            var fileVector = DenseVector.OfEnumerable(file);

            // Process image

            var start = DateTime.Now;
            metrics.StartProcessing();
            
            var processed = CGNRSolver.Solve(fileVector);

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
                User = userId,
                Image = Convert.ToBase64String(imgReturn),
                CPU = cpuUsage,
                Memory = ramUsage,
                Iterations = iterations,
                TimeElapsed = elapsedTime.TotalMilliseconds,
                StartOperation = start,
                EndOperation = end
            };

            //return File(imgReturn, "image/bmp");

            return Ok(resultObject);
        }
    }
}