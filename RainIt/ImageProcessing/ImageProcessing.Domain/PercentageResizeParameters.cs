using System;

namespace ImageProcessing.Domain
{
	public class PercentageResizeParameters : ResizeParameters
	{
		public PercentageResizeParameters(){
			DefaultHeightUnit = 100;
			DefaultWidthUnit= 100;
		}

		public int TargetWidthPercentage {
			get{
				return TargetWidthUnit;
			}
			set{
				TargetWidthUnit = value;
			}
		}
		public int TargetHeightPercentage {
			get{
				return TargetHeightUnit;
			}
			set{
				TargetHeightUnit = value;
			}
		}
	}
}

