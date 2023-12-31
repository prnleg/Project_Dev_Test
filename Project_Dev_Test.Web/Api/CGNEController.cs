﻿using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Web.Models;
using Project_Dev_Test.Web.Readers;
using Project_Dev_Test.Web.Service;

namespace Project_Dev_Test.Web.Api
{
    public class CGNEController : Controller
    {
        private readonly AlgorithmService service;

        public CGNEController(AlgorithmService service)
        {
            this.service = service;
        }

        [HttpPost("{userId}/CGNE-SolverImageSignal")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CGNEImageSignal([FromRoute] int userId)
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

            var resultObject = await service.GetResult(fileVector, AlgorithmEnum.CGNE);
            service.SaveResult(resultObject, userId);

            resultObject.User = userId;

            return Ok(resultObject);
        }
    }
}
