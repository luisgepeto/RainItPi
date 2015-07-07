

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using ImageProcessing.Domain;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IPatternManager
    {
        StatusMessage AddUserPattern(ImageDetails pattern, PatternUploadModel patternUploadModel, bool canUpdate = false);
        StatusMessage SetToTest(ImageDetails pattern, string base64Image, List<Guid> deviceIdentifierList);

        UploadConstraintParameters GetUploadConstraintParameters();
        List<PatternDTO> GetUserPatterns();
        PatternDTO GetUserPattern(int patternId);
        List<PatternDTO> GetAllPatterns();
        string GetPatternUrl(int patternId);
        StatusMessage UpdateUserPattern(ImageDetails pattern, string fileName);
        StatusMessage DeleteUserPattern(int patternId);
    }
}
