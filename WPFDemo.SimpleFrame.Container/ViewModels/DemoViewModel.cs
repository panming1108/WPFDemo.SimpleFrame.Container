using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.Infra.Tools;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class DemoViewModel : BaseViewModel, IDemoViewModel
    {
        public NotifyTaskCompletion<int> UrlByteCount { get; private set; }

        public DemoViewModel()
        {
            UrlByteCount = new NotifyTaskCompletion<int>(MyStaticService.CountBytesInUrlAsync("https://www.Baidu.com"));
        }

        protected override async Task Loaded()
        {
            await TaskEx.FromResult(0);
        }

        protected override async Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }

    public static class MyStaticService
    {
        public static async Task<int> CountBytesInUrlAsync(string url)
        {
            // Artificial delay to show responsiveness.
            await TaskEx.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
            // Download the actual data and count it.
            using (var client = new HttpClient())
            {
                var data = await client.GetByteArrayAsync(url).ConfigureAwait(false);
                return data.Length;
            }
        }
    }
}
