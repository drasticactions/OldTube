using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace OldTube.Pages
{
    public class ResultsModel : PageModel
    {
        private YoutubeClient youtubeClient;

        public ResultsModel(YoutubeClient youtubeClient)
        {
            this.youtubeClient = youtubeClient;
        }

        public bool IsEmpty { get; set; }

        public int CurrentPage { get; private set; }

        public List<Video> Videos { get; set; }

        public string SearchString { get; set; }

        public async Task OnGet(string search, int p)
        {
            if (string.IsNullOrEmpty(search))
            {
                this.IsEmpty = true;
                return;
            }
            this.SearchString = search;

            this.CurrentPage = p;
            if (this.CurrentPage < 1)
            {
                this.CurrentPage = 1;
            }

            this.Videos = (await this.youtubeClient.Search.GetVideosAsync(this.SearchString, this.CurrentPage, 1)).ToList();
        }
    }
}
