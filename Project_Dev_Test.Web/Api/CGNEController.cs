using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;
using Project_Dev_Test.Web.Readers;

namespace Project_Dev_Test.Web.Api
{

    public class CGNEController : Controller
    {
        private readonly IRepository _repository;

        public CGNEController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CGNE-SolverImageSignal")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CGNEImageSignal(List<IFormFile> imagesCSV)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            // Ler Matriz
            List<double[]> matrixRead = new List<double[]>();
            matrixRead = CSVFileReader.CSVFileReaderListDouble(imagesCSV[0]);
            double[,] matrixImage = CSVFileReader.toMatrix(matrixRead);

            // Ler Sinal (vetor)
            List<double> signalRead = new List<double>();
            signalRead = CSVFileReader.CSVFileReaderVector(imagesCSV[1]);
            double[] signalImage = CSVFileReader.toVector(signalRead);

            // resolução em CGNR
            //var result = CGNRSolver.Solve(matrixImage);

            return Ok(/*result*/);
        }
    }
}
