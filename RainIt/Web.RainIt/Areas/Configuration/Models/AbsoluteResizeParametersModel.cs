using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.RainIt.Areas.Configuration.Models
{
    public class AbsoluteResizeParametersModel
    {
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }

        public int? TargetWidth { get; set; }
        public int? TargetHeight { get; set; }
        public bool IsProportional { get; set; }
    }
}