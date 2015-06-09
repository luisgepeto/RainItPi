

using System.Collections.Generic;
using System.Drawing;
using System.Web;
using ImageProcessing.Domain;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IPatternManager
    {
        StatusMessage AddUserPattern(ImageDetails pattern, string fileName, bool canUpdate = false);
        List<PatternList> GetUserPatterns();
        StatusMessage UpdateUserPattern(ImageDetails pattern, string fileName);
        StatusMessage DeleteUserPattern(int patternId);
        
    }
}
