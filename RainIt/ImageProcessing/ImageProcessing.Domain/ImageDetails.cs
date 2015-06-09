using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Domain
{
    public class ImageDetails
    {
        public ImageDetails(Image image)
        {
            Image = image;
            ImageStream = GetStream();
        }
        
        public Image Image { get; set; }

        public long FileSize
        {
            get
            {
                return  ImageStream.Length; 
            }
        }

        public int Width
        {
            get { return Image.Width; }
        }

        public int Height
        {
            get { return Image.Height; }
        }

        private MemoryStream _imageStream;
        public MemoryStream ImageStream
        {
            get
            {
                _imageStream.Position = 0;
                return _imageStream; 
            }
            private set
            {
                _imageStream = value; 
            }
        }

        public string FileType
        {
            get { return new ImageFormatConverter().ConvertToString(Image.RawFormat); }
        }

        private MemoryStream GetStream(){
	         var imageStream = new MemoryStream();
            Image.Save(imageStream, ImageFormat.Png);
	        return imageStream;
	    }
    }
}
