using System;
using System.Drawing;
using ImageProcessing.Domain;
using ImageProcessing.Business.Interfaces;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using RainIt.Domain.DTO;

namespace ImageProcessing.Business
{
	public class ImageManager : IImageManager
	{
        public Image LoadFromUrl(string url)
        {
            Image imageFromUrl = null;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebReponse.GetResponseStream();
            if (stream != null)
            {
                imageFromUrl = Image.FromStream(stream);
            }
            return imageFromUrl;
        }

	    public string ConvertToBase64(string imagePath)
	    {
            var memoryStream = new MemoryStream(new WebClient().DownloadData(imagePath));
            var base64String = Convert.ToBase64String(memoryStream.ToArray());
	        return base64String;
	    }

	    public Image Resize(Image image, ResizeParameters resizeParameters){
			Type parametersType = resizeParameters.GetType ();
			if (typeof(AbsoluteResizeParameters).IsAssignableFrom (parametersType)) {
				return Resize (image, (AbsoluteResizeParameters)resizeParameters);
			}
			if (typeof(PercentageResizeParameters).IsAssignableFrom (parametersType)) {
				return Resize (image, (PercentageResizeParameters)resizeParameters);
			}
			return image;
		}

		private Image Resize(Image image, AbsoluteResizeParameters absoluteResizeParameters){
			var newSize = new Size (absoluteResizeParameters.TargetWidth, absoluteResizeParameters.TargetHeight);
			return (Image)new Bitmap (image, newSize);
		}

		private Image Resize(Image image, PercentageResizeParameters percentageResizeParameters){
			var newWidth = (percentageResizeParameters.TargetWidthPercentage * image.Width) / 100;
			var newHeight = (percentageResizeParameters.TargetHeightPercentage * image.Height) / 100;
			var newSize = new Size (newWidth, newHeight);
			return (Image)new Bitmap (image, newSize);
		}

	    public bool[,] GetUpsideDownBooleanMatrix(Image image){
			var bitMapInputImage = new Bitmap (image);
			var booleanMatrix = GetUpsideDownBooleanMatrix (bitMapInputImage);
			return booleanMatrix;
		}

		private bool[,] GetUpsideDownBooleanMatrix(Bitmap bitmap){
			var newBooleanMatrix = new bool[bitmap.Width, bitmap.Height];
			for (int i = bitmap.Height-1; i >= 0 ; i--) {
				for (int j = 0; j < bitmap.Width; j++) {
					var currentPixel = bitmap.GetPixel (j, i);
					var booleanCurrentPixel = GetBoolean (currentPixel);
					newBooleanMatrix [i, j] = booleanCurrentPixel;
				}
			}
			return newBooleanMatrix;
		}

		private bool GetBoolean(Color color){
			return IsWhite (color) ? true : false;
		}

		private bool IsWhite(Color color){
			return color.R == 255 && color.G == 255 && color.B == 255;
		}

		public Image GetBlackWhite(Image image, bool isInverted = false, double thresholdPercentage = 50, ColorRelativeWeight colorRelativeWeight = null ){
			var bwParameters = new BlackWhiteConversionParameters (true, isInverted, thresholdPercentage);
			var colorWeight = colorRelativeWeight ?? new ColorRelativeWeight ();
			return GetGrayScale (image, colorWeight, bwParameters);
		}

		public Image GetGrayScale(Image image, ColorRelativeWeight colorRelativeWeight = null, BlackWhiteConversionParameters blackWhiteParameters = null){
			var colorWeight = colorRelativeWeight ?? new ColorRelativeWeight ();
			var bwParameters = blackWhiteParameters ?? new BlackWhiteConversionParameters ();
			var grayScaleMatrix = GetGrayScaleMatrix (image, colorWeight, bwParameters);
			return GetImage (grayScaleMatrix);
		}

		private Color[,] GetGrayScaleMatrix(Image image,  ColorRelativeWeight relativeWeight, BlackWhiteConversionParameters blackWhiteParameters){
			var bitMapInputImage = new Bitmap (image);
			var grayScaleMatrix = GetGrayScaleMatrix (bitMapInputImage,relativeWeight, blackWhiteParameters);
			return grayScaleMatrix;
		}

		private Color[,] GetGrayScaleMatrix(Bitmap bitmap,  ColorRelativeWeight relativeWeight, BlackWhiteConversionParameters blackWhiteParameters){
			var newPixelMatrix = new Color[bitmap.Width, bitmap.Height];
			for (int i = 0; i < bitmap.Width; i++) {
				for (int j = 0; j < bitmap.Height; j++) {
					var currentPixel = bitmap.GetPixel (i, j);
					var grayScaledCurrentPixel = GetGrayScale (currentPixel, relativeWeight, blackWhiteParameters);
					newPixelMatrix [i, j] = grayScaledCurrentPixel;
				}
			}
			return newPixelMatrix;
		}

		private Color GetGrayScale(Color color, ColorRelativeWeight relativeWeight, BlackWhiteConversionParameters blackWhiteParameters){
			var averageColorValue = GetAverage (color, relativeWeight);
			if (blackWhiteParameters.IsBlackWhite) {
				averageColorValue = averageColorValue >= blackWhiteParameters.ThresholdValue ? 255 : 0;
				averageColorValue = blackWhiteParameters.IsInverted ? Math.Abs (averageColorValue - 255) : averageColorValue;
			}
			return Color.FromArgb (averageColorValue, averageColorValue, averageColorValue);
		}

		private int GetAverage (Color color, ColorRelativeWeight relativeWeight){
			return (int)((color.R*relativeWeight.RWeight + color.G*relativeWeight.GWeight + color.B*relativeWeight.BWeight)/relativeWeight.TotalSum);
		}

		private Image GetImage(Color[,] pixelMatrix){
			var bitMapOutputImage = new Bitmap (pixelMatrix.GetLength(0), pixelMatrix.GetLength(1));
			CopyMatrixTo (bitMapOutputImage, pixelMatrix);
			return (Image)bitMapOutputImage;
		}

		private void CopyMatrixTo(Bitmap outputBitmap, Color[,] pixelMatrix){
			for (int i = 0; i < outputBitmap.Width; i++) {
				for (int j = 0; j < outputBitmap.Height; j++) {
					outputBitmap.SetPixel (i, j, pixelMatrix[i,j]);
				}
			}
		}

	    public bool TryParseImage(string base64Image, out ImageDetails imageDetails)
	    {
	        var newPatternUploadModel = new PatternUploadModel() {Base64Image = base64Image};
	        return TryParseImage(newPatternUploadModel, out imageDetails);
	    }

	    public bool TryParseImage(PatternUploadModel patternUploadModel, out ImageDetails imageDetails)
	    {
	        imageDetails = null;
            var canParse = false;
            try
            {
                var image = ParseImage(patternUploadModel.Base64Image);
                if (patternUploadModel.AbsoluteResizeParameters != null)
                {
                    image = (Bitmap) Resize(image, patternUploadModel.AbsoluteResizeParameters);    
                }
                imageDetails = new ImageDetails(image);
                imageDetails.Base64Image = ConvertToBase64(image);
                canParse = true;
            }
            catch (Exception ex)
            {
                canParse = false;
            }
            return canParse;
	    }


	    private Bitmap ParseImage(string base64Image)
	    {
	        Byte[] bitmapData = new Byte[base64Image.Length];
	        bitmapData = Convert.FromBase64String(FixBase64ForImage(base64Image));
	        MemoryStream streamBitmap = new MemoryStream(bitmapData);
	        var image = new Bitmap((Bitmap) Image.FromStream(streamBitmap));
	        return image;
	    }

	    private string FixBase64ForImage(string imageString) 
        { 
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(imageString,imageString.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty); 
            return sbText.ToString(); 
        }

         public string ConvertToBase64(Image image)
	    {
	        using (MemoryStream ms = new MemoryStream())
            {
            image.Save(ms, ImageFormat.Jpeg);
            byte[] imageBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
            }
	    }

	}
}

