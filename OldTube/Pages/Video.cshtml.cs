using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace OldTube.Pages
{
    public class VideoModel : PageModel
    {
        private YoutubeClient youtubeClient;

        public VideoModel(YoutubeClient youtubeClient)
        {
            this.youtubeClient = youtubeClient;
        }

        public Video Video { get; private set; }

        public YoutubeExplode.Channels.Channel Channel { get; private set; }

        public async Task OnGet(string id)
        {
            this.Video = await this.youtubeClient.Videos.GetAsync(id);
            this.Channel = await this.youtubeClient.Channels.GetAsync(this.Video.ChannelId);
        }
    }
}
