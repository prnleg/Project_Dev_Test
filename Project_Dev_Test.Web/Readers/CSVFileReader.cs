using Project_Dev_Test.Web.Algorithm;
using System.Collections.Generic;

namespace Project_Dev_Test.Web.Readers
{
    public class CSVFileReader
    {
        public CSVFileReader() { }
        public static List<double[]> CSVFileReaderListDouble(IFormFile csvFile) 
        {            
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            List<double[]> matrixRead = new List<double[]>();

            using (StreamReader reader = new StreamReader(csvFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(',');
                    matrixRead.Add(Array.ConvertAll(line, Double.Parse));
                }
            }

            return matrixRead;
        }
        public static double[,] toMatrix(List<double[]> matrixRead = null)
        {

            double[,] matrixImage = new double[matrixRead.Count, matrixRead[0].Length];

            if (matrixRead.Count > 1)
                for (int i = 0; i < matrixRead.Count; i++)
                    for (int j = 0; j < matrixRead[0].Length; j++)
                        matrixImage[i, j] = matrixRead[i][j];
            else
                for (int i = 0; i < matrixRead[0].Length; i++)
                    matrixImage[i, 0] = matrixRead[0][i];

            return matrixImage;
        }


        public static List<double> CSVFileReaderVector(IFormFile csvFile)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            List<double> signalRead = new List<double>();
            using (StreamReader reader = new StreamReader(csvFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    signalRead.Add(double.Parse(line));
                }
            }

            double[] signalImage = new double[signalRead.Count];

            for (int i = 0; i < signalRead.Count; i++)
                signalImage[i] = signalRead[i];

            return signalRead;
        }

        public static double[] toVector(List<double> signalRead)
        {
            double[] signalImage = new double[signalRead.Count];

            for (int i = 0; i < signalRead.Count; i++)
                signalImage[i] = signalRead[i];

            return signalImage;
        }

    }

}
