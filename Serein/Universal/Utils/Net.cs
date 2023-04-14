using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Utils
{
    internal static class Net
    {
        /// <summary>
        /// Http客户端
        /// </summary>
        private static HttpClient HttpClient = new();

        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="accept">Header - Accept</param>
        /// <param name="userAgent">Header - UserAgent</param>
        /// <returns>正文</returns>
        public static async Task<HttpResponseMessage> Get(string url, string accept = null, string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33")
        {
            if (accept != null)
            {
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }
            HttpClient.DefaultRequestHeaders.AcceptCharset.Clear();
            HttpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
            HttpClient.DefaultRequestHeaders.Remove("user-agent");
            HttpClient.DefaultRequestHeaders.Add("user-agent", userAgent);
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            Logger.Output(Base.LogType.DetailDebug, "Response Headers\n", response.Headers.ToString());
            Logger.Output(Base.LogType.DetailDebug, "Content\n", await response.Content.ReadAsStringAsync());
            return response;
        }
    }
}
