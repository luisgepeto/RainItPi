using System;

namespace ImageProcessing.Domain
{
	public abstract class ResizeParameters
	{
		public bool IsProportional {
			get;
			set;
		}

		protected int DefaultHeightUnit {
			get;
			set;
		}

		protected int DefaultWidthUnit {
			get;
			set;
		}

		private double WidthToHeightDefaultRatio{
			get{ 
				return DefaultWidthUnit / (double)DefaultHeightUnit;
			}
		}
		private double HeightToWidthDefaultRatio{
			get{ 
				return DefaultHeightUnit / (double)DefaultWidthUnit;
			}
		}

		private int? _targetWidthUnit {
			get;
			set;
		}

		protected int TargetWidthUnit {
			get{
				return _targetWidthUnit ?? DefaultWidthUnit;
			}
			set{
				_targetWidthUnit = value;
				if (IsProportional)
					_targetHeightUnit = (int)(HeightToWidthDefaultRatio*value);
			}
		}


		private int? _targetHeightUnit {
			get;
			set;
		}

		protected int TargetHeightUnit {
			get{
				return _targetHeightUnit ?? DefaultHeightUnit;
			}
			set{
				_targetHeightUnit = value;
				if (IsProportional)
					_targetWidthUnit = (int)(WidthToHeightDefaultRatio*value);
			}
		}	
	}
}

