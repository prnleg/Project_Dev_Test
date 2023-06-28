using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Project_Dev_Test.Web.Readers;

namespace Project_Dev_Test.Web
{
    public static class Helpers
    {
        public static class MatrixModel
        {
            public static Matrix<double> H = null;
            public static Matrix<double> Ht = null;

            static MatrixModel()
            {
                // nada!
            }

            public static void Initialize()
            {
                var a = Directory.GetCurrentDirectory();
                if (!File.Exists("./H-1.csv"))
                {
                    throw new FileNotFoundException("H-1.csv file not found under execution path");
                }

                if (H == null)
                {
                    var hFile = File.OpenRead("H-1.csv");
                    var formFile = new FormFile(hFile, 0, hFile.Length, "H-1.csv", "H-1.csv");

                    var hMatrix = CSVFileReader.CSVFileReaderListDouble(formFile);
                    var hMatrixProc = CSVFileReader.toMatrix(hMatrix);

                    H = ConvertToMatrix(hMatrixProc);
                }

                if (Ht == null)
                {
                    Ht = H.Transpose();
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

    }
}
