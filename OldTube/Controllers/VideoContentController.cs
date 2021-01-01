using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace OldTube.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoContentController : ControllerBase
    {
        private HttpClient client = new HttpClient();
        private YoutubeClient youtube = new YoutubeClient();
        private string assemblyPath;
        private string outputPath;

        public VideoContentController()
        {
            assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            outputPath = Path.Join(assemblyPath, "output");
            Directory.CreateDirectory(outputPath);
        }

        /// <summary>
        /// Get Request.
        /// </summary>
        /// <returns>Action Result.</returns>
        [Route("{id?}")]
        [HttpGet]
        public async Task<ActionResult> Get(string id)
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(id);
            var streamInfo = streamManifest.GetMuxed().Where(n => n.Container == Container.Mp4);
            var test = streamInfo.FirstOrDefault();
            var path = Path.Join(outputPath, $"{id}.wmv");
            if (! System.IO.File.Exists(path))
            {
                var conversionResult = await FFmpeg.Conversions.New()
        .AddParameter($"-i {test.Url}")
        .AddParameter($"-user-agent \"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2593.0 Safari/537.36\"")
        .AddParameter($"-vf scale=320:-1")
        .AddParameter($"-b:v 24k")
        .AddParameter($"-vcodec wmv1")
        .AddParameter($"-acodec wmav1")
        .AddParameter($"-b:a 24k")
        .SetOutput(path)
        .Start();
            }
            return File(System.IO.File.OpenRead(path), "video/x-ms-wmv", true);
        }
    }
}
