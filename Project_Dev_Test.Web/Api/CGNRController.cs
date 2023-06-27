using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;
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
                    ImageConverter converter = new ImageConverter();
                    var imgReturn = (byte[])converter.ConvertTo(ToBitmap(matrixImage), typeof(byte[]));
                    return File(imgReturn, "image/bmp");
                }
            }

            // Ler Sinal (vetor)
            List<double> signalRead = new List<double>();
            signalRead = CSVFileReader.CSVFileReaderVector(imagesCSV[1]);
            double[] signalImage = CSVFileReader.toVector(signalRead) ?? null;

            // resolução em CGNR

        private unsafe Bitmap ToBitmap(double[,] rawImage)
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

        public struct ColorARGB
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

            public ColorARGB(Color color)
            {
                A = color.A;
                R = color.R;
                G = color.G;
                B = color.B;
            }

            public ColorARGB(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public Color ToColor()
            {
                return Color.FromArgb(A, R, G, B);
            }
        }
    }
}
