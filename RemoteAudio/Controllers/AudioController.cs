using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RemoteAudio.Controllers
{
    public class AudioController : ApiController
    {
        [Route("audio")]
        [HttpGet]
        public HttpResponseMessage Get() {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)(AudioServer.getInstance().WriteToStream), new MediaTypeHeaderValue("audio/mpeg"));
            response.Headers.Add("Cache-Control", "no-cache");
            return response;
        }

        //[Route("web")]
        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    var response = Request.CreateResponse();
        //    return response;
        //}
    }
}
