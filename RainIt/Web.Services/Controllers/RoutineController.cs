using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageProcessing.Business.Interfaces;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;

namespace Web.Services.Controllers
{
    public class RoutineController : ApiController
    {
        public IRoutineManager RoutineManager { get; set; }
        public IImageManager ImageManager { get; set; }

        public RoutineController(IRoutineManager routineManager, IImageManager imageManager)
        {
            RoutineManager = routineManager;
            ImageManager = imageManager;
        }

        [HttpGet]
        public HttpResponseMessage GetActiveRoutine()
        {
            var routine = RoutineManager.GetActiveUserRoutine();
            var responseStream = new List<bool[,]>();
            foreach (var routinePattern in routine.RoutinePatternDTOs)
            {
                Image image;
                if (!TryLoadImageFromPath(routinePattern.PatternDTO.Path, out image))
                    Request.CreateResponse(HttpStatusCode.Unauthorized, "The image could not be read from path");
                var imageAsMatrix = ImageManager.GetUpsideDownBooleanMatrix(image);
                responseStream.Add(imageAsMatrix);
            }
            return Request.CreateResponse(HttpStatusCode.OK, responseStream);
        }

        private bool TryLoadImageFromPath(string path, out Image image)
        {
            image = null;
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(path);
            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            if (stream != null)
            {
                image = Image.FromStream(stream);
                return true;
            }
            return false;
        }
    }
}
