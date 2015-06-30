using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using ImageProcessing.Domain;

namespace Web.RainIt.Models.Pattern
{
    public class PatternUploadModel
    {
        public string FileName { get; set; }
        public string Base64Image { get; set; }
        public AbsoluteResizeParameters AbsoluteResizeParameters { get; set; }
    }
}