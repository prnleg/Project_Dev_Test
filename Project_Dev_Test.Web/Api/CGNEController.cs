using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Project_Dev_Test.Core.Interfaces;
using System.Net;

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
        //[VerifyImageExists]
        public IActionResult CGNEImage(List<IFormFile> imagesCSV = null)
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
    }
}
