using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp.Core.Services
{
    public class WebService
    {
        static WebService instance;
        public static WebService Instance
        {
            get
            {
                if (instance == null)
                    instance = new WebService();
                return instance;
            }
        }

        HttpClient _client = new HttpClient();

        public WebService()
        {
            _client.Timeout = new TimeSpan(0, 0, 60);
        }

        public Task<HttpResponseMessage> Get(string uri)
        {
            return Get(new Uri(uri));
        }

        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            HttpResponseMessage response = null;
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                response = await _client.SendAsync(request);
            }
            return response;
        }

        //public Task<HttpResponseMessage> Post(string uri, object content)
        //{
        //    return Post(uri, content, null);
        //}

        //public Task<T> Post<T>(string uri, object content)
        //{
        //    return Post<T>(uri, content, null);
        //}

        //public async Task<HttpResponseMessage> Post(string uri, object content, IEnumerable<KeyValuePair<string, string>> headers)
        //{
        //    HttpResponseMessage response = null;
        //    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri))
        //    {
        //        if (headers != null)
        //        {
        //            foreach (var header in headers)
        //                request.Headers.Add(header.Key, header.Value);
        //        }

        //        string json = JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });

        //        var contentBytes = json.ToByteArray();
        //        var deflatedContentBytes = contentBytes.Deflate();

        //        if (deflatedContentBytes.Length < contentBytes.Length)
        //        {
        //            request.Headers.Add(HttpHeaders.Compression, HttpHeaders.Values.Deflate);
        //            request.Content = new ByteArrayContent(deflatedContentBytes);
        //        }
        //        else
        //        {
        //            request.Content = new ByteArrayContent(contentBytes);
        //        }

        //        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        //        // request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        //        response = await client.SendAsync(request);
        //    }

        //    return response;
        //}

        //public async Task<T> Post<T>(string uri, object content, IEnumerable<KeyValuePair<string, string>> headers)
        //{
        //    using (HttpResponseMessage response = await Post(uri, content, headers))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseStringContent = await GetHttpContentAsString(response);
        //            if (responseStringContent != null)
        //                return JsonConvert.DeserializeObject<T>(responseStringContent);
        //            else return default(T);
        //        }
        //        else
        //        {
        //            return default(T);
        //        }
        //    }
        //}

        //async Task<string> GetHttpContentAsString(HttpResponseMessage message)
        //{
        //    string contentString = null;
        //    var contentBytes = await message.Content.ReadAsByteArrayAsync();

        //    if (message.Headers.TryGetValues(HttpHeaders.Compression, out IEnumerable<string> compressionValues))
        //    {
        //        if (compressionValues != null && compressionValues.Any(x => x == HttpHeaders.Values.Deflate))
        //        {
        //            contentString = contentBytes.Inflate().AsString();
        //        }
        //    }

        //    if (string.IsNullOrEmpty(contentString))
        //    {
        //        contentString = contentBytes.AsString();
        //    }

        //    return contentString;
        //}
    }
}