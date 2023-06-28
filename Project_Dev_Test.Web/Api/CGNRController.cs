using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Web.Algorithm;
using Project_Dev_Test.Web.Readers;
using System.Collections.Generic;
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
        public async Task<IActionResult> CGNRImageSignal()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            MemoryStream mstream = new MemoryStream();
            await HttpContext.Request.Body.CopyToAsync(mstream);

            var formFile = new FormFile(mstream, 0, mstream.Length, "image-csv", "image-csv");

            if (formFile == null)
            {
                throw new ArgumentNullException("Image CSV is null or empty");
            }

            var file = CSVFileReader.CSVFileReaderVector(formFile);
            var fileVector = DenseVector.OfEnumerable(file);

            var processedImage = CGNRSolver.Solve(fileVector).Item1;

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
                    imgReturn = converter.ConvertTo(ToBitmap(imageArray), typeof(byte[])) as byte[];
                }
            }

            var base64 = Convert.ToBase64String(imgReturn);

            return File(imgReturn, "image/bmp");
        }

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
