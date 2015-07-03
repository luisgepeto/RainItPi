﻿using System;
using System.Web.Http;
using System.Web.Mvc;
using ImageProcessing.Business.Interfaces;
using ImageProcessing.Domain;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;
using Web.RainIt.Areas.Configuration.Models;
using Web.RainIt.Models.Pattern;

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
            var constraintParameters = PatternManager.GetUploadConstraintParameters();
            return View(constraintParameters);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Add(PatternUploadModel patternUploadModel)
        {
            ImageDetails imageDetails;
            StatusMessage canAdd = null;
            if (ImageManager.TryParseImage(patternUploadModel.Base64Image, out imageDetails, patternUploadModel.AbsoluteResizeParameters))
            {
                canAdd = PatternManager.AddUserPattern(imageDetails, patternUploadModel.FileName);
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
        public JsonResult GetGrayScaleFor([FromBody] string base64Image,
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

        [System.Web.Mvc.HttpPost]
        public JsonResult Delete(int patternId)
        {
            StatusMessage canAdd = PatternManager.DeleteUserPattern(patternId);
            TempData["StatusMessage"] = canAdd;
            return Json(Url.Action("Index", "Pattern" , new{area = "Configuration"}));
        }
    }
}