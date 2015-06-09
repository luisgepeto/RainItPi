﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using ImageProcessing.Domain;

namespace ImageProcessing.Business.Interfaces
{
	public interface IImageManager
	{
		bool[,] GetUpsideDownBooleanMatrix (Image image);
		Image GetBlackWhite (Image image, bool isInverted = false, double thresholdPercentage = 50, ColorRelativeWeight colorRelativeWeight = null);
		Image GetGrayScale(Image image, ColorRelativeWeight colorRelativeWeight = null, BlackWhiteConversionParameters blackWhiteParameters = null);
	    bool TryParseImage(string base64Image, out ImageDetails imageDetails);
	    string ConvertToBase64(Image image);
	}
}

