using Dorkari.Framework.Web.MediaTypeFormatters;
using Dorkari.Framework.Web.Models.Enums;
using Dorkari.Helpers.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Dorkari.Framework.Web.Communicators
{
    public static class RestClient
    {
        static IEnumerable<MediaTypeFormatter> _formatters;

        static RestClient()
        {
            _formatters = new List<MediaTypeFormatter>() { 
                new JsonMediaTypeFormatter(), 
                new XmlMediaTypeFormatter(),
                new ProtobufMediaTypeFormatter()
            };
        }

        public static TResult Get<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null)
        {
            if (parameters != null)
            {
                uri += uri.Contains("?") ? "&" : "?";
                uri += string.Join("&", parameters.Select(queryParam => queryParam.Key + "=" + queryParam.Value));
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType.GetEnumAttribute<AcceptHeaderAttribute>().Value));

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(_formatters).Result;
                    }
                    throw new Exception(string.Format("GET call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }            
        }

        //TODO: needs more work
        public static TResult Post<TData, TResult>(string url, TData data)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.PostAsJsonAsync(url, data).Result)
            {
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<TResult>(_formatters).Result;
                }
                return default(TResult);
            }          
        }


        public static T Delete<T>(string url)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.DeleteAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>(_formatters).Result;
                }
                return default(T);
            }
        }

        public static T Put<T, Y>(string url, Y data)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.PutAsJsonAsync(url, data).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>(_formatters).Result;
                }
                return default(T);
            }
        }
    }
}