using System;

namespace ImageProcessing.Domain
{
	public class AbsoluteResizeParameters : ResizeParameters
	{
		public int OriginalWidth {
			get{ 
				return DefaultWidthUnit;
			}
			set{ 
				DefaultWidthUnit = value;
			}
		}

		public int OriginalHeight {
			get{ 
				return DefaultHeightUnit;
			}
			set{ 
				DefaultHeightUnit = value;
			}
		}

		public int TargetWidth{
			get{
				return TargetWidthUnit;
			}
			set{
				TargetWidthUnit = value;
			}
		}
		public int TargetHeight{
			get{
				return TargetHeightUnit;
			}
			set{
				TargetHeightUnit = value;
			}
		}

	}
}

