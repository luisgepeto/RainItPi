using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using ImageProcessing.Business;
using ImageProcessing.Domain;

namespace ImageProcessing.Bussiness.Tests
{
	[TestFixture ()]
	public class ImageManagerUnitTests
	{
		public ImageManager Manager {
			get;
			set;
		}

		[TestFixtureSetUp()]
		public void TestInitialize(){
			Manager = new ImageManager ();
		}

		public bool GetGrayScale_Default_Image(int imageId){
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			//Act
			var grayScale = Manager.GetGrayScale (testImage);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}", @"TestImages\Image",imageId,"GrayScale.jpeg"));
			grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			return File.Exists (resultingImagePath);
		}

		[Test ()][Ignore()]
		public void GetGrayScale_Default_Image0()
		{
			var testResult = GetGrayScale_Default_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_Default_Image1()
		{
			var testResult = GetGrayScale_Default_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_Default_Image2()
		{
			var testResult = GetGrayScale_Default_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_Default_Image3()
		{
			var testResult = GetGrayScale_Default_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_Default_Image4()
		{
			var testResult = GetGrayScale_Default_Image (4);
			Assert.IsTrue (testResult);
		}

		public bool GetGrayScale_CustomLevels_Image(int imageId){
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			//Act
			var colorRelativeWeight = new ColorRelativeWeight (60, 30, 10);
			var grayScale = Manager.GetGrayScale (testImage, colorRelativeWeight);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"GrayScale","_60","_30","_10",".jpeg"));
			grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			return File.Exists (resultingImagePath);
		}

		[Test ()][Ignore()]
		public void GetGrayScale_CustomLevels_Image0()
		{
			var testResult = GetGrayScale_CustomLevels_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_CustomLevels_Image1()
		{
			var testResult = GetGrayScale_CustomLevels_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_CustomLevels_Image2()
		{
			var testResult = GetGrayScale_CustomLevels_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_CustomLevels_Image3()
		{
			var testResult = GetGrayScale_CustomLevels_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetGrayScale_CustomLevels_Image4()
		{
			var testResult = GetGrayScale_CustomLevels_Image (4);
			Assert.IsTrue (testResult);
		}


		public bool GetBlackWhite_Default_Image(int imageId){
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			//Act
			var grayScale = Manager.GetBlackWhite (testImage);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}", @"TestImages\Image",imageId,"BlackWhite.jpeg"));
			grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			return File.Exists (resultingImagePath);
		}

		[Test ()][Ignore()]
		public void GetBlackWhite_Default_Image0()
		{
			var testResult = GetBlackWhite_Default_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_Default_Image1()
		{
			var testResult = GetBlackWhite_Default_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_Default_Image2()
		{
			var testResult = GetBlackWhite_Default_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_Default_Image3()
		{
			var testResult = GetBlackWhite_Default_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_Default_Image4()
		{
			var testResult = GetBlackWhite_Default_Image (4);
			Assert.IsTrue (testResult);
		}

		public bool GetBlackWhite_CustomLevels_Image(int imageId){
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			//Act
			var customLevels = new ColorRelativeWeight (60, 30, 10);
			var grayScale = Manager.GetBlackWhite (testImage,false, 50, customLevels);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"BlackWhite","_60","_30","_10",".jpeg"));
			grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			return File.Exists (resultingImagePath);
		}

		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_Image0()
		{
			var testResult = GetBlackWhite_CustomLevels_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_Image1()
		{
			var testResult = GetBlackWhite_CustomLevels_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_Image2()
		{
			var testResult = GetBlackWhite_CustomLevels_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_Image3()
		{
			var testResult = GetBlackWhite_CustomLevels_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_Image4()
		{
			var testResult = GetBlackWhite_CustomLevels_Image (4);
			Assert.IsTrue (testResult);
		}

		public bool GetBlackWhite_CustomSpan_Image (int imageId)
		{
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var cumulativeAssert = true;
			//Act
			for (var i = 0; i <= 10; i++) {
				var currentPercentage = i +90;
				var grayScale = Manager.GetBlackWhite (testImage,false, currentPercentage);
				var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}", @"TestImages\Image",imageId,"BlackWhite",currentPercentage,".jpeg"));
				grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
				//Assert
				cumulativeAssert &= File.Exists (resultingImagePath);
			}
			return cumulativeAssert;
		}

		[Test ()][Ignore()]
		public void GetBlackWhite_CustomSpan_Image0 ()
		{
			var testResult = GetBlackWhite_CustomSpan_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomSpan_Image1 ()
		{
			var testResult = GetBlackWhite_CustomSpan_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomSpan_Image2 ()
		{
			var testResult = GetBlackWhite_CustomSpan_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomSpan_Image3 ()
		{
			var testResult = GetBlackWhite_CustomSpan_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomSpan_Image4 ()
		{
			var testResult = GetBlackWhite_CustomSpan_Image (4);
			Assert.IsTrue (testResult);
		}

		public bool GetBlackWhite_CustomLevels_CustomSpan_Image (int imageId)
		{
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var cumulativeAssert = true;
			//Act
			for (var i = 0; i <= 10; i++) {
				var currentPercentage = i*10;
				var customLevels = new ColorRelativeWeight (60, 30, 10);
				var grayScale = Manager.GetBlackWhite (testImage, false, currentPercentage, customLevels);
				var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", @"TestImages\Image",imageId,"BlackWhite","_60","_30","_10_",currentPercentage,".jpeg"));
				grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
				//Assert
				cumulativeAssert &= File.Exists (resultingImagePath);
			}
			return cumulativeAssert;
		}

		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_CustomSpan_Image0 ()
		{
			var testResult = GetBlackWhite_CustomLevels_CustomSpan_Image (0);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_CustomSpan_Image1 ()
		{
			var testResult = GetBlackWhite_CustomLevels_CustomSpan_Image (1);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_CustomSpan_Image2 ()
		{
			var testResult = GetBlackWhite_CustomLevels_CustomSpan_Image (2);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_CustomSpan_Image3 ()
		{
			var testResult = GetBlackWhite_CustomLevels_CustomSpan_Image (3);
			Assert.IsTrue (testResult);
		}
		[Test ()][Ignore()]
		public void GetBlackWhite_CustomLevels_CustomSpan_Image4 ()
		{
			var testResult = GetBlackWhite_CustomLevels_CustomSpan_Image (4);
			Assert.IsTrue (testResult);
		}


		[Test ()][Ignore()]
		public void GetUpsideDownBooleanMatrix_CustomLevels_Image1()
		{
			var imageId = 1;
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			//Act
			var customLevels = new ColorRelativeWeight (60, 30, 10);
			var grayScale = Manager.GetBlackWhite (testImage,false, 50, customLevels);
			var booleanMatrix = Manager.GetUpsideDownBooleanMatrix (grayScale);
		}

		public bool GetBlackWhite_Inverted_CustomSpan_Image (int imageId)
		{
			//configure
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var cumulativeAssert = true;
			//Act
			for (var i = 0; i <= 10; i++) {
				var currentPercentage = i *10;
				var grayScale = Manager.GetBlackWhite (testImage, true, currentPercentage);
				var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}", @"TestImages\Image",imageId,"BlackWhite_Inverted",currentPercentage,".jpeg"));
				grayScale.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
				//Assert
				cumulativeAssert &= File.Exists (resultingImagePath);
			}
			return cumulativeAssert;
		}

		[Test ()][Ignore()]
		public void GetBlackWhite_Inverted_CustomSpan_Image4 ()
		{
			var testResult = GetBlackWhite_Inverted_CustomSpan_Image (4);
			Assert.IsTrue (testResult);
		}

		private string GetTestingRootPath(){
			return Path.GetDirectoryName (
					Path.GetDirectoryName (
						Path.GetDirectoryName (
							Assembly.GetExecutingAssembly ().Location)));
		}

		[Test()]
		public void Resize_Absolute_Proportional_Bigger_Image_0(){
			//configure
			var imageId = 0;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var absoluteParameters = new AbsoluteResizeParameters () {
				IsProportional = true,
				OriginalHeight = testImage.Height,
				OriginalWidth = testImage.Width,
				TargetHeight = 50
			};

			//act
			var scaledImage = Manager.Resize (testImage, absoluteParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Absolute","Proportional","Bigger",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}

		[Test()]
		public void Resize_Absolute_Proportional_Smaller_Image_4(){
			//configure
			var imageId = 4;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var absoluteParameters = new AbsoluteResizeParameters () {
				IsProportional = true,
				OriginalHeight = testImage.Height,
				OriginalWidth = testImage.Width,
				TargetWidth = 450
			};

			//act
			var scaledImage = Manager.Resize (testImage, absoluteParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Absolute","Proportional","Smaller",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}

		[Test()]
		public void Resize_Absolute_NonProportional_Custom_Image_4(){
			//configure
			var imageId = 4;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var absoluteParameters = new AbsoluteResizeParameters () {
				IsProportional = false,
				OriginalHeight = testImage.Height,
				OriginalWidth = testImage.Width,
				TargetHeight = 540,
				TargetWidth = 1800
			};

			//act
			var scaledImage = Manager.Resize (testImage, absoluteParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Absolute","NonProportional","Custom",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}

		[Test()]
		public void Resize_Percentage_Proportional_Bigger_Image_0(){
			//configure
			var imageId = 0;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var percentageParameters = new PercentageResizeParameters () {
				IsProportional = true,
				TargetHeightPercentage = 200
			};

			//act
			var scaledImage = Manager.Resize (testImage, percentageParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Percentage","Proportional","Bigger",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}

		[Test()]
		public void Resize_Percentage_Proportional_Smaller_Image_4(){
			//configure
			var imageId = 4;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var percentageParameters = new PercentageResizeParameters () {
				IsProportional = true,
				TargetWidthPercentage = 50
			};

			//act
			var scaledImage = Manager.Resize (testImage, percentageParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Percentage","Proportional","Smaller",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}

		[Test()]
		public void Resize_Percentage_NonProportional_Custom_Image_4(){
			//configure
			var imageId = 4;
			var relativeImgPath = String.Format("{0}{1}{2}", @"TestImages\Image",imageId,".jpeg");
			var fullImagePath = Path.Combine(GetTestingRootPath(), relativeImgPath);
			var testImage = Image.FromFile (fullImagePath);

			var percentageParameters = new PercentageResizeParameters () {
				IsProportional = false,
				TargetWidthPercentage = 60,
				TargetHeightPercentage = 120
			};

			//act
			var scaledImage = Manager.Resize (testImage, percentageParameters);
			var resultingImagePath = Path.Combine (GetTestingRootPath (), String.Format("{0}{1}{2}{3}{4}{5}{6}", @"TestImages\Image",imageId,"Resize","Percentage","NonProportional","Custom",".jpeg"));
			scaledImage.Save (resultingImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

			//Assert
			Assert.IsTrue (File.Exists (resultingImagePath));
		}
	}
}

