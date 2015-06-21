using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageProcessing.Business.Interfaces;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    public class PatternController : ApiController
    {
        public IPatternManager PatternManager { get; set; }
        public IImageManager ImageManager { get; set; }

        public PatternController(IPatternManager patternManager, IImageManager imageManager)
        {
            PatternManager = patternManager;
            ImageManager = imageManager;
        }

        [HttpPost]
        public IHttpActionResult Transform(int patternId)
        {
            var patternUrl = PatternManager.GetPatternUrl(patternId);
            var imageToTransform = ImageManager.LoadFromUrl(patternUrl);
            var matrixToDisplay = ImageManager.GetUpsideDownBooleanMatrix(imageToTransform);
            return Ok(matrixToDisplay);
        }

    }
}
