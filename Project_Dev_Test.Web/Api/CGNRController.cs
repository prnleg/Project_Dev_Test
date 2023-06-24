using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;

namespace Project_Dev_Test.Web.Api
{

    public class CGNEController : Controller
    {
        private readonly IRepository _repository;

        public CGNEController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("TestCSVMatrix")]
        //[VerifyImageFile]
        public IActionResult TestCSVMatrix(List<IFormFile> imagesCSV)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            List<double[]> list2D = new List<double[]>();

            using (StreamReader reader = new StreamReader(imagesCSV[0].OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(';');
                    list2D.Add(Array.ConvertAll(line, Double.Parse));
                }
            }

            double[,] listA = new double[list2D[0].Length, list2D.Count];
            
            if (list2D.Count > 1)
                for (int i = 0; i < list2D[0].Length; i++)
                    for (int j = 0; j < list2D.Count; j++)
                        listA[i, j] = list2D[i][j];
            else
                for (int i = 0; i < list2D[0].Length; i++)
                    listA[i, 0] = list2D[0][i];

            var A = Matrix<double>.Build.DenseOfArray(listA);
            list2D = new List<double[]>();

            using (StreamReader reader = new StreamReader(imagesCSV[1].OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(';');
                    list2D.Add(Array.ConvertAll(line, Double.Parse));
                }
            }

            double[,] listB = new double[list2D[0].Length, list2D.Count];

            if (list2D.Count > 1)
                for (int i=0; i < list2D[0].Length; i++) 
                    for (int j=0; j < list2D.Count; j++)
                        listB[i,j] = list2D[i][j];
            else
                for (int i = 0;i < list2D[0].Length; i++)
                    listB[i,0] = list2D[0][i];

            var B = Matrix<double>.Build.DenseOfArray(listB);

            try
            {
                var vectorResult = A * B;
                return Ok(vectorResult);
                
            }

            catch { }
            
            try {
                var vectorResult = B * A;
                return Ok(vectorResult);
            }

            catch { }

            return Ok("Não foi possivel realizar o calculo");
        }

        [HttpPost("CGNRSolverImageSignal")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CGNRImageSignal(List<IFormFile> imagesCSV)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            List<double[]> matrixRead = new List<double[]>();
            List<double> signalRead = new List<double>();

            using (StreamReader reader = new StreamReader(imagesCSV[0].OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(',');
                    matrixRead.Add(Array.ConvertAll(line, Double.Parse));
                }
            }

            double[,] matrixImage = new double[matrixRead.Count, matrixRead[0].Length];

            if (matrixRead.Count > 1)
                for (int i = 0; i < matrixRead.Count; i++)
                    for (int j = 0; j < matrixRead[0].Length; j++)
                        matrixImage[i, j] = matrixRead[i][j];
            else
                for (int i = 0; i < matrixRead[0].Length; i++)
                    matrixImage[i, 0] = matrixRead[0][i];


            using (StreamReader reader = new StreamReader(imagesCSV[1].OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    signalRead.Add(double.Parse(line));
                }
            }

            double[] signalImage = new double[signalRead.Count];

            for (int i = 0; i < matrixRead.Count; i++)
                signalImage[i] = signalRead[i];

            var result = CGNRSolver.Solve(matrixImage, signalImage);

            return Ok(result);
        }
    }
}
