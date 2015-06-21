using System;
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
        public StatusMessage AddUserPattern(ImageDetails pattern, string fileName, bool canUpdate = false)
        {
            if (!IsFileSizeValid(pattern)) return StatusMessage.WriteError("The file size is not valid.");
            if (!AreDimensionsValid(pattern)) return StatusMessage.WriteError("The file dimensions are not valid.");
            int patternId;
            return !DoesUserFileNameExist(fileName, out patternId)
                ? AddPattern(pattern, fileName)
                : StatusMessage.WriteError("The selected file name is already in use");
        }
        private StatusMessage AddPattern(ImageDetails pattern, string fileName)
        {
            string generatedUri;
            var canAddToCloud = AzureCloudContext.TryAddToCloudInContainer(pattern.ImageStream, fileName,
                RainItContext.CurrentUser.Username, out generatedUri);
            return !canAddToCloud.IsError ? AddToDatabase(pattern, fileName, generatedUri) : canAddToCloud;
        }
        private StatusMessage AddToDatabase(ImageDetails imageDetails, string fileName, string filePath)
        {
            try
            {
                var patternToAdd = new Pattern()
                {
                    BytesFileSize = imageDetails.FileSize,
                    FileType = imageDetails.FileType,
                    Height = imageDetails.Height,
                    Name = fileName,
                    Path = filePath,
                    Width = imageDetails.Width
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
        public List<PatternDTO> GetUserPatterns()
        {
            return RainItContext.UserPatternSet.Select(p => new PatternDTO()
            {
                Name = p.Name,
                PatternId = p.PatternId,
                Path = p.Path
            }).ToList();
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
        public StatusMessage UpdateUserPattern(ImageDetails pattern, string fileName)
        {
            if (!IsFileSizeValid(pattern)) return StatusMessage.WriteError("The file size is not valid.");
            if (!AreDimensionsValid(pattern)) return StatusMessage.WriteError("The file dimensions are not valid.");
            int patternId;
            return DoesUserFileNameExist(fileName, out patternId)
                ? UpdatePattern(pattern, fileName, patternId)
                : StatusMessage.WriteError("The selected file name was not found");
        }
        private StatusMessage UpdatePattern(ImageDetails pattern, string fileName, int patternId)
        {
            string generatedUri;
            var canUpdateInCloud = AzureCloudContext.TryUpdateToCloudInContainer(pattern.ImageStream,
                fileName,
                RainItContext.CurrentUser.Username, out generatedUri);
            return !canUpdateInCloud.IsError 
                ? UpdateInDatabase(pattern, patternId, generatedUri) 
                : canUpdateInCloud;
        }
        private StatusMessage UpdateInDatabase(ImageDetails imageDetails, int patternId, string filePath)
        {
            try
            {
                var patternToUpdate = RainItContext.UserPatternSet.Single(p => p.PatternId == patternId);
                patternToUpdate.BytesFileSize = imageDetails.FileSize;
                patternToUpdate.FileType = imageDetails.FileType;
                patternToUpdate.Height = imageDetails.Height;
                patternToUpdate.Path = filePath;
                patternToUpdate.Width = imageDetails.Width;
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
        public StatusMessage DeleteUserPattern(string fileName)
        {
            int patternId;
            return DoesUserFileNameExist(fileName, out patternId)
                ? DeletePattern(fileName, patternId)
                : StatusMessage.WriteError("The selected pattern was not found");
        }

        public StatusMessage DeleteUserPattern(int patternId)
        {
            string fileName;
            return DoesUserPatternExist(patternId, out fileName)
                ? DeletePattern(fileName, patternId)
                : StatusMessage.WriteError("The selected pattern was not found");
        }

        private StatusMessage DeletePattern(string fileName, int patternId)
        {
            var canDeleteFromCloud = AzureCloudContext.TryDeleteFromCloudInContainer(fileName,
                RainItContext.CurrentUser.Username);
            return !canDeleteFromCloud.IsError 
                ? DeleteFromDatabase(patternId) 
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
        private bool DoesUserPatternExist(int patternId, out string fileName)
        {
            fileName = String.Empty;
            var pattern = RainItContext.UserPatternSet.SingleOrDefault(p => p.PatternId == patternId);

            if (pattern == null) return false;

            fileName = pattern.Name;
            return true;
        }
        #endregion
    }
}
