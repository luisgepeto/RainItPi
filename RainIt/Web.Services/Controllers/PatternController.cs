using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageProcessing.Business.Interfaces;
using ImageProcessing.Domain;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;
using Web.Infrastructure.Attributes;

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

        [HttpGet]
        [WebApiOutputCache(120, 60, false)]
        public IHttpActionResult Transform(int patternId, [FromBody] ConversionParameterDTO conversionParameterDTO)
        {
            var patternUrl = PatternManager.GetPatternUrl(patternId);
            var imageToTransform = ImageManager.LoadFromUrl(patternUrl);
            var blackWhiteImage = ImageManager.GetBlackWhite(imageToTransform, conversionParameterDTO.IsInverted,
                conversionParameterDTO.ThresholdPercentage,
                new ColorRelativeWeight((int)conversionParameterDTO.RWeight, (int)conversionParameterDTO.GWeight,
                    (int)conversionParameterDTO.BWeight));
            var matrixToDisplay = ImageManager.GetUpsideDownBooleanMatrix(blackWhiteImage);
            return Ok(matrixToDisplay);
        }

    }
}
