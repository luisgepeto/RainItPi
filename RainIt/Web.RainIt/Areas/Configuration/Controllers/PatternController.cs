using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using ImageProcessing.Business.Interfaces;
using ImageProcessing.Domain;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;
using Web.RainIt.Areas.Configuration.Models;

namespace Web.RainIt.Areas.Configuration.Controllers
{
    public class PatternController : Controller
    {
        public IPatternManager PatternManager;
        public IImageManager ImageManager;

        public PatternController(IPatternManager patternManager, IImageManager imageManager)
        {
            PatternManager = patternManager;
            ImageManager = imageManager;
        }

        public ActionResult Index()
        {
            var patterns = PatternManager.GetUserPatterns();
            return View(patterns);
        }

        public ActionResult Add()
        {
            ViewBag.ConstraintParameters = PatternManager.GetUploadConstraintParameters();
            var pattern = new PatternDTO()
            {
                ConversionParameterDTO = new ConversionParameterDTO()
                {
                    RWeight = 50,
                    GWeight = 50,
                    BWeight = 50,
                    ThresholdPercentage = 50
                }
            };
            return View(pattern);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ConstraintParameters = PatternManager.GetUploadConstraintParameters();
            var pattern = PatternManager.GetUserPattern(id);
            pattern.Base64Image = ImageManager.ConvertToBase64(pattern.Path);
            return View("Add", pattern);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult Add(PatternUploadModel patternUploadModel)
        {
            ImageDetails imageDetails;
            StatusMessage canAdd = null;
            if (ImageManager.TryParseImage(patternUploadModel, out imageDetails))
            {
                if (patternUploadModel.PatternId == 0)
                    canAdd = PatternManager.AddUserPattern(imageDetails, patternUploadModel);
                else
                    canAdd = PatternManager.UpdateUserPattern(imageDetails, patternUploadModel);
            }
            else
            {
                canAdd = new StatusMessage()
                {
                    IsError = true,
                    Message = "The selected file is not an image."
                };
            }
            TempData["StatusMessage"] = canAdd;
            if (canAdd.IsError)
            {
                TempData["PatternUploadModel"] = patternUploadModel;
                return Json(Url.Action("Add", "Pattern", new{area = "Configuration"}));
            }
            return Json(Url.Action("Index", new { area = "Configuration" }));
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetGrayscaleFor([FromBody] string base64Image,
            [FromUri] ColorRelativeWeight colorRelativeWeight)
        {
            ImageDetails imageDetails;
            if (ImageManager.TryParseImage(base64Image, out imageDetails))
            {
                var grayscale = ImageManager.GetGrayScale(imageDetails.Image, colorRelativeWeight);
                var grayScaleFile = ImageManager.ConvertToBase64(grayscale);
                return Json(new {grayScaleFile}, JsonRequestBehavior.DenyGet);
            }
            return Json(false, JsonRequestBehavior.DenyGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetBlackWhiteFor([FromBody] string base64Image, [FromUri] bool isInverted,
            [FromUri] double thresholdPercentage)
        {
            ImageDetails imageDetails;
            if (ImageManager.TryParseImage(base64Image, out imageDetails))
            {
                var blackWhite = ImageManager.GetBlackWhite(imageDetails.Image, isInverted, thresholdPercentage);
                var blackWhiteFile = ImageManager.ConvertToBase64(blackWhite);
                return Json(new {blackWhiteFile}, JsonRequestBehavior.DenyGet);
            }
            return Json(false, JsonRequestBehavior.DenyGet);
        }

        public JsonResult GetBlackWhiteFor(int patternId)
        {
            var pattern = PatternManager.GetUserPattern(patternId);
            var imageDetails = ImageManager.GetImageFromPath(pattern.Path);
            var blackWhiteImage = ImageManager.GetBlackWhite(imageDetails.Image,
                                        pattern.ConversionParameterDTO.IsInverted, 
                                        pattern.ConversionParameterDTO.BWeight,
                                        new ColorRelativeWeight((int) pattern.ConversionParameterDTO.RWeight,(int) pattern.ConversionParameterDTO.GWeight, (int) pattern.ConversionParameterDTO.BWeight));
            var blackWhiteFile = ImageManager.ConvertToBase64(blackWhiteImage);
            return Json(new {patternId, blackWhiteFile}, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Delete(int patternId)
        {
            StatusMessage canAdd = PatternManager.DeleteUserPattern(patternId);
            TempData["StatusMessage"] = canAdd;
            return Json(Url.Action("Index", "Pattern" , new{area = "Configuration"}));
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Test(PatternUploadModel patternUploadModel, List<Guid> deviceIdentifierList)
        {
            StatusMessage canSet = new StatusMessage();
            ImageDetails imageDetails;
            if (deviceIdentifierList == null || !deviceIdentifierList.Any())
                canSet = StatusMessage.WriteError("No device was selected");
            if (!canSet.IsError && ImageManager.TryParseImage(patternUploadModel, out imageDetails))
            {
                canSet = PatternManager.SetToTest(imageDetails, deviceIdentifierList);
            }
            else
            {
                canSet = StatusMessage.WriteError("The selected file is not an image");    
            }
            return Json(new { canSet }, JsonRequestBehavior.DenyGet);
        }
    }
}