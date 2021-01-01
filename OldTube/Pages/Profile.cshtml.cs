using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace OldTube.Pages
{
    public class ProfileModel : PageModel
    {
        private YoutubeClient youtubeClient;

        public ProfileModel(YoutubeClient youtubeClient)
        {
            this.youtubeClient = youtubeClient;
        }
        public YoutubeExplode.Channels.Channel Channel { get; private set; }
        public List<Video> Videos { get; private set; }

        public int CurrentPage { get; private set; }

        public async Task OnGet(string id, int p)
        {
            this.CurrentPage = p;
            if (this.CurrentPage < 1)
            {
                this.CurrentPage = 1;
            }

            this.Channel = await this.youtubeClient.Channels.GetAsync(id);
            var videos = await this.youtubeClient.Channels.GetUploadsAsync(this.Channel.Id);
            this.Videos = videos.Skip(this.CurrentPage > 1 ? this.CurrentPage * 10 : 0).Take(this.CurrentPage * 10).ToList();
        }
    }
}
