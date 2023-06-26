using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;
using Project_Dev_Test.Web.Readers;

using System.Drawing.Imaging;
using System.Drawing;
using System;
using MathNet.Numerics;

namespace Project_Dev_Test.Web.Api
{

    public class CGNRController : Controller
    {
        private readonly IRepository _repository;

        public CGNRController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CGNR-SolverImageSignal")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CGNRImageSignal(List<IFormFile> imagesCSV)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            // Ler Matriz
            List<double[]> matrixRead = new List<double[]>();
            matrixRead = CSVFileReader.CSVFileReaderListDouble(imagesCSV[0]);
            double[,] matrixImage = CSVFileReader.toMatrix(matrixRead) ?? null;

            Bitmap bitmap;
            unsafe
            {
                fixed (double* intPtr = &matrixImage[0, 0])
                {
                    bitmap = new Bitmap(matrixImage.GetLength(0), matrixImage.GetLength(1), matrixImage.GetLength(0)*4, PixelFormat.Format32bppRgb, new IntPtr(intPtr)) ?? null;
                    
                    if(bitmap != null)
                        return Ok(bitmap);
                }
            }

            // Ler Sinal (vetor)
            List<double> signalRead = new List<double>();
            signalRead = CSVFileReader.CSVFileReaderVector(imagesCSV[1]);
            double[] signalImage = CSVFileReader.toVector(signalRead) ?? null;

            // resolução em CGNR
            return Json(CGNRSolver.Solve(matrixImage, signalImage));
        }
    }
}
