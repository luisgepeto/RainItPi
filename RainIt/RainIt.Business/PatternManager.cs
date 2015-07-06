﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ImageProcessing.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;


namespace RainIt.Business
{
    public class PatternManager : IPatternManager
    {
        public IRainItContext RainItContext { get; set; }
        public IAzureCloudContext AzureCloudContext { get; set; }
        public PatternManager(IRainItContext rainItContext, IAzureCloudContext azureCloudContext)
        {
            RainItContext = rainItContext;
            AzureCloudContext = azureCloudContext;
        }

        #region CREATE METHODS
        public StatusMessage AddUserPattern(ImageDetails pattern, PatternUploadModel patternUploadModel)
        {
            if (!IsFileSizeValid(pattern)) return StatusMessage.WriteError("The file size is not valid.");
            if (!AreDimensionsValid(pattern)) return StatusMessage.WriteError("The file dimensions are not valid.");
            int patternId;
            return !DoesUserFileNameExist(patternUploadModel.FileName, out patternId)
                ? AddPattern(pattern, patternUploadModel)
                : StatusMessage.WriteError("The selected file name is already in use");
        }

        private StatusMessage AddPattern(ImageDetails pattern, PatternUploadModel patternUploadModel)
        {
            string generatedUri;
            var canAddToCloud = AzureCloudContext.TryAddToCloudInContainer(pattern.ImageStream, patternUploadModel.FileName,
                RainItContext.CurrentUser.Username, out generatedUri);
            return !canAddToCloud.IsError ? AddToDatabase(pattern, patternUploadModel, generatedUri) : canAddToCloud;
        }
        private StatusMessage AddToDatabase(ImageDetails imageDetails, PatternUploadModel patternUploadModel, string filePath)
        {
            try
            {
                var patternToAdd = new Pattern()
                {
                    BytesFileSize = imageDetails.FileSize,
                    FileType = imageDetails.FileType,
                    Height = imageDetails.Height,
                    Name = patternUploadModel.FileName,
                    Path = filePath,
                    Width = imageDetails.Width,
                    ConversionParameter = new ConversionParameter()
                    {
                        RWeight = patternUploadModel.ColorRelativeWeight.RWeight,
                        GWeight = patternUploadModel.ColorRelativeWeight.GWeight,
                        BWeight = patternUploadModel.ColorRelativeWeight.BWeight,
                        IsInverted = patternUploadModel.BlackWhiteConversionParameters.IsInverted,
                        ThresholdPercentage = patternUploadModel.BlackWhiteConversionParameters.ThresholdPercentage
                    }
                };
                var user = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username);
                patternToAdd.UserId = user.UserId;

                RainItContext.PatternSet.Add(patternToAdd);
                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("The pattern was successfully added to the database");
            }
            catch (Exception e)
            {
                return StatusMessage.WriteError("An unexpected error occurred. Please try again");
            }
        }
        #endregion

        #region READ METHODS
        public UploadConstraintParameters GetUploadConstraintParameters()
        {
            return new UploadConstraintParameters()
            {
                MaxFileSize = int.Parse(ConfigurationManager.AppSettings["MaxPatternByteCount"]),
                MaxHeight = int.Parse(ConfigurationManager.AppSettings["MaxPatternPixelHeight"]),
                MaxWidth = int.Parse(ConfigurationManager.AppSettings["MaxPatternPixelWidth"]),
                MaxNameLength = 50
            };
        }
        public List<PatternDTO> GetUserPatterns()
        {
            return RainItContext.UserPatternSet.Select(p => new PatternDTO()
            {
                Name = p.Name,
                PatternId = p.PatternId,
                Path = p.Path
            }).ToList();
        }
        public PatternDTO GetUserPattern(int patternId)
        {
            return RainItContext.UserPatternSet.Where(p => p.PatternId == patternId).Select(p => new PatternDTO()
            {
                Name = p.Name,
                PatternId = p.PatternId,
                Path = p.Path,
                Height = p.Height,
                Width = p.Width,
                ConversionParameterDTO = new ConversionParameterDTO()
                {
                    RWeight = p.ConversionParameter.RWeight,
                    GWeight = p.ConversionParameter.GWeight,
                    BWeight = p.ConversionParameter.BWeight,
                    IsInverted = p.ConversionParameter.IsInverted,
                    ThresholdPercentage = p.ConversionParameter.ThresholdPercentage
                }
            }).Single();
        }
        public List<PatternDTO> GetAllPatterns()
        {
            return RainItContext.PatternSet.Select(p => new PatternDTO()
            {
                Name = p.Name,
                PatternId = p.PatternId,
                Path = p.Path
            }).ToList();
        }

        public string GetPatternUrl(int patternId)
        {
            var pattern =
                RainItContext.PatternSet.Single(p => p.PatternId == patternId);
            return pattern.Path;
        }
        #endregion

        #region UPDATE METHODS
        public StatusMessage UpdateUserPattern(ImageDetails pattern, PatternUploadModel patternUploadModel)
        {
            if (!IsFileSizeValid(pattern)) return StatusMessage.WriteError("The file size is not valid.");
            if (!AreDimensionsValid(pattern)) return StatusMessage.WriteError("The file dimensions are not valid.");
            Pattern patternToEdit;
            return DoesUserPatternExist(patternUploadModel.PatternId, out patternToEdit)
                ? UpdatePattern(pattern, patternToEdit, patternUploadModel)
                : StatusMessage.WriteError("The selected file name was not found");
        }
        private StatusMessage UpdatePattern(ImageDetails imagePattern, Pattern pattern, PatternUploadModel patternUploadModel)
        {
            string generatedUri;
            var canUpdateInCloud = AzureCloudContext.TryUpdateToCloudInContainer(imagePattern.ImageStream,
                pattern.Name,patternUploadModel.FileName,RainItContext.CurrentUser.Username, out generatedUri);
            return !canUpdateInCloud.IsError 
                ? UpdateInDatabase(imagePattern, patternUploadModel, generatedUri) 
                : canUpdateInCloud;
        }
        private StatusMessage UpdateInDatabase(ImageDetails imageDetails, PatternUploadModel patternUploadModel, string generatedUri)
        {
            try
            {
                var patternToUpdate = RainItContext.UserPatternSet.Single(p => p.PatternId == patternUploadModel.PatternId);
                patternToUpdate.BytesFileSize = imageDetails.FileSize;
                patternToUpdate.FileType = imageDetails.FileType;
                patternToUpdate.Height = imageDetails.Height;
                patternToUpdate.Path = generatedUri;
                patternToUpdate.Width = imageDetails.Width;
                patternToUpdate.Name = patternUploadModel.FileName;
                patternToUpdate.ConversionParameter.RWeight = patternUploadModel.ColorRelativeWeight.RWeight;
                patternToUpdate.ConversionParameter.GWeight = patternUploadModel.ColorRelativeWeight.GWeight;
                patternToUpdate.ConversionParameter.BWeight = patternUploadModel.ColorRelativeWeight.BWeight;
                patternToUpdate.ConversionParameter.ThresholdPercentage = patternUploadModel.BlackWhiteConversionParameters.ThresholdPercentage;
                patternToUpdate.ConversionParameter.IsInverted = patternUploadModel.BlackWhiteConversionParameters.IsInverted;

                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("The pattern was successfully updated in the database");
            }
            catch (Exception e)
            {
                return StatusMessage.WriteError("An unexpected error occurred. Please try again");
            }
        }
        #endregion

        #region DELETE METHODS
        public StatusMessage DeleteUserPattern(int patternId)
        {
            Pattern patternToDelete;
            return DoesUserPatternExist(patternId, out patternToDelete)
                ? DeletePattern(patternToDelete)
                : StatusMessage.WriteError("The selected pattern was not found");
        }

        private StatusMessage DeletePattern(Pattern pattern)
        {
            var canDeleteFromCloud = AzureCloudContext.TryDeleteFromCloudInContainer(pattern.Name,
                RainItContext.CurrentUser.Username);
            return !canDeleteFromCloud.IsError 
                ? DeleteFromDatabase(pattern.PatternId) 
                : canDeleteFromCloud;
        }

        private StatusMessage DeleteFromDatabase(int patternId)
        {
            try
            {
                var patternToDelete = RainItContext.UserPatternSet.Single(p => p.PatternId == patternId);
                RainItContext.PatternSet.Attach(patternToDelete);
                RainItContext.PatternSet.Remove(patternToDelete);
                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("The pattern was successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusMessage.WriteError("An unexpected error occurred. Please try again.");
            }
        }
        #endregion

        #region Helper Methods
        private bool IsFileSizeValid(ImageDetails image)
        {
            var maxSize = int.Parse(ConfigurationManager.AppSettings["MaxPatternByteCount"]);
            return image.FileSize <= maxSize;
        }
        private bool AreDimensionsValid(ImageDetails image)
        {
            var maxHeight = int.Parse(ConfigurationManager.AppSettings["MaxPatternPixelHeight"]);
            var maxWidth = int.Parse(ConfigurationManager.AppSettings["MaxPatternPixelWidth"]);
            return image.Width <= maxWidth && image.Height <= maxHeight;
        }
        private bool DoesUserFileNameExist(string fileName, out int patternId)
        {
            patternId = 0;
            var pattern = RainItContext.UserPatternSet.SingleOrDefault(p => p.Name == fileName);
            
            if (pattern == null) return false;

            patternId = pattern.PatternId;
            return true;
        }
        private bool DoesUserPatternExist(int patternId, out Pattern outPattern)
        {
            outPattern = RainItContext.UserPatternSet.SingleOrDefault(p => p.PatternId == patternId);
            return outPattern != null;
        }
        #endregion
    }
}
