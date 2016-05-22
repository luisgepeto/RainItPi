using System;

namespace ImageProcessing.Domain
{
	public class BlackWhiteConversionParameters
	{
		public BlackWhiteConversionParameters ()
		{
		}
		public BlackWhiteConversionParameters (bool isBlackWhite, bool isInverted = false, double thresholdPercentage = 50)
		{
			IsBlackWhite = isBlackWhite;
			ThresholdPercentage = thresholdPercentage;
			IsInverted = isInverted;
		}

		public bool IsBlackWhite {
			get;
			set;
		}

		public bool IsInverted {
			get;
			set;
		}
		public double ThresholdValue{
			get { return (int)(255 * (ThresholdPercentage / 100.0)); }
		}
		public double ThresholdPercentage {
			get;
			set;
		}
	}
}

