using Serein.Utils.Output;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Serein.Utils
{
    internal static class Net
    {
        /// <summary>
        /// Http客户端
        /// </summary>
        private static HttpClient _httpClient = new();

        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="accept">Header - Accept</param>
        /// <param name="userAgent">Header - UserAgent</param>
        /// <returns>正文</returns>
        public static async Task<HttpResponseMessage> Get(
            string url,
            string? accept = null,
            string userAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33"
        )
        {
            if (!string.IsNullOrEmpty(accept))
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(accept)
                );
            }
            _httpClient.DefaultRequestHeaders.AcceptCharset.Clear();
            _httpClient.DefaultRequestHeaders.AcceptCharset.Add(
                new StringWithQualityHeaderValue("UTF-8")
            );
            _httpClient.DefaultRequestHeaders.Remove("user-agent");
            _httpClient.DefaultRequestHeaders.Add("user-agent", userAgent);
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Logger.Output(
                Base.LogType.DetailDebug,
                "Response Headers\n",
                response.Headers.ToString()
            );
            Logger.Output(
                Base.LogType.DetailDebug,
                "Content\n",
                await response.Content.ReadAsStringAsync()
            );
            return response;
        }
    }
}
