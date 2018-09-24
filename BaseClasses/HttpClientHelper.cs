using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class HttpClientHelper
    {
        public const string ContentTypeForm = "application/x-www-form-urlencoded";
        public const string ContentTypeJson = "application/json";
        public const string EncodingUtf8 = "utf-8";
        public const string EncodingGbk = "gbk";


        public async Task<string> Download(string url, string filePath)
        {
            string fileExt = url.Substring(url.Length - 4, 4);
            string fileName = CypherUtility.Md5(url) + fileExt;
            string newFileName = filePath + CypherUtility.Md5(url) + fileExt;
            HttpClient client = new HttpClient();
            byte[] bytes = await client.GetByteArrayAsync(url);
            FileStream fs = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
            fs.Close();
            return fileName;
        }

        
        public async Task<string> HttpGet(string url, string encoding = "")
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (encoding != "")
            {
                var httpContentType = response.Content.Headers.ContentType;
                if (httpContentType != null)
                {
                    httpContentType.CharSet = encoding;
                }
            }
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> HttpPost(string url, Dictionary<string, string> data, string encoding = "", string contentType = "")
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(data);
            content.Headers.ContentType = contentType != "" ? new MediaTypeHeaderValue(contentType) : new MediaTypeHeaderValue(ContentTypeForm);
            if (encoding != "")
            {
                content.Headers.ContentType.CharSet = encoding;
            }
	        var request = new HttpRequestMessage(HttpMethod.Post, url) {Content = content};
	        HttpResponseMessage response = await client.SendAsync(request);
            if (encoding != "")
            {
                var httpContentType = response.Content.Headers.ContentType;
                if (httpContentType != null)
                {
                    httpContentType.CharSet = encoding;
                }
            }
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<string> HttpPost(string url, string data, string encoding = "", string contentType = "")
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(data);
            content.Headers.ContentType = contentType != "" ? new MediaTypeHeaderValue(contentType) : new MediaTypeHeaderValue(ContentTypeForm);
            if (encoding != "")
            {
                content.Headers.ContentType.CharSet = encoding;
            }
	        var request = new HttpRequestMessage(HttpMethod.Post, url) {Content = content};
	        HttpResponseMessage response = await client.SendAsync(request);
            if (encoding != "")
            {
                var httpContentType = response.Content.Headers.ContentType;
                if (httpContentType != null)
                {
                    httpContentType.CharSet = encoding;
                }
            }
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<dynamic> HttpPost_1(string url, string data, string encoding = "", string contentType = "", string token = "")
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(data);
            content.Headers.ContentType = contentType != "" ? new MediaTypeHeaderValue(contentType) : new MediaTypeHeaderValue(ContentTypeForm);
            if (encoding != "")
            {
                content.Headers.ContentType.CharSet = encoding;
            }
            content.Headers.Add("User-Token", token);
	        var request = new HttpRequestMessage(HttpMethod.Post, url) {Content = content};
	        HttpResponseMessage response = await client.SendAsync(request);
            if (encoding != "")
            {
                var httpContentType = response.Content.Headers.ContentType;
                if (httpContentType != null)
                {
                    httpContentType.CharSet = encoding;
                }
            }
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            HttpStatusCode statusCode = response.StatusCode;
            return new
            {
                Content = result,
                StatusCode = statusCode
            };
        }

    }
}
