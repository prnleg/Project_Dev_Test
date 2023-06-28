using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Project_Dev_Test.Web.Readers;
using System.Drawing.Imaging;
using System.Drawing;
using Project_Dev_Test.Web.Models;

namespace Project_Dev_Test.Web
{
    public static class Helpers
    {
        public static class MatrixModel
        {
            public static Matrix<double> H1 = null;
            public static Matrix<double> H1t = null;

            public static Matrix<double> H2 = null;
            public static Matrix<double> H2t = null;

            static MatrixModel() { }

            public static void Initialize()
            {
                var a = Directory.GetCurrentDirectory();
                if (!File.Exists("./H-1.csv") || !File.Exists("./H-2.csv"))
                {
                    throw new FileNotFoundException("H file not found under execution path");
                }

                if (H1 == null)
                {
                    var hFile = File.OpenRead("./H-1.csv");
                    var formFile = new FormFile(hFile, 0, hFile.Length, "H-1.csv", "H-1.csv");

                    var hMatrix = CSVFileReader.CSVFileReaderListDouble(formFile);
                    var hMatrixProc = CSVFileReader.toMatrix(hMatrix);

                    H1 = ConvertToMatrix(hMatrixProc);
                }

                if (H1t == null)
                {
                    H1t = H1.Transpose();
                }

                if (H2 == null)
                {
                    var hFile = File.OpenRead("./H-2.csv");
                    var formFile = new FormFile(hFile, 0, hFile.Length, "H-2.csv", "H-2.csv");

                    var hMatrix = CSVFileReader.CSVFileReaderListDouble(formFile);
                    var hMatrixProc = CSVFileReader.toMatrix(hMatrix);

                    H2 = ConvertToMatrix(hMatrixProc);
                }

                if (H2t == null)
                {
                    H2t = H2.Transpose();
                }
            }

            private static Matrix<double> ConvertToMatrix(double[,] array)
            {
                int rows = array.GetLength(0);
                int columns = array.GetLength(1);

                Matrix<double> matrix = DenseMatrix.Create(rows, columns, (i, j) => array[i, j]);
                return matrix;
            }
        }

        static double[,] TransposeMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] transposedMatrix = new double[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposedMatrix[j, i] = matrix[i, j];
                }
            }

            return transposedMatrix;
        }

        public static unsafe Bitmap ToBitmap(double[,] rawImage)
        {
            int width = rawImage.GetLength(1);
            int height = rawImage.GetLength(0);

            Bitmap Image = new Bitmap(width, height);
            BitmapData bitmapData = Image.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            ColorARGB* startingPosition = (ColorARGB*)bitmapData.Scan0;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double color = rawImage[i, j];
                    byte rgb = (byte)(color * 255);

                    ColorARGB* position = startingPosition + j + i * width;
                    position->A = 255;
                    position->R = rgb;
                    position->G = rgb;
                    position->B = rgb;
                }

            Image.UnlockBits(bitmapData);
            return Image;
        }

    }
}
