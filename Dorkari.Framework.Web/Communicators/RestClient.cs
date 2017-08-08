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
        static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter();
        static readonly XmlMediaTypeFormatter _xmlFormatter = new XmlMediaTypeFormatter();
        static readonly ProtobufMediaTypeFormatter _protobufFormatter = new ProtobufMediaTypeFormatter();

        //TODO: needs testing
        public static TResult Get<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            Func<HttpClient, HttpResponseMessage> httpGet = httpClient => httpClient.GetAsync(uri).Result;
            return ExecuteHttp<TResult>(uri, httpGet, "GET", acceptType, queryParams, headers);
        }

        //TODO: needs testing
        public static TResult Post<TData, TResult>(string uri, TData postData, MediaType contentType = MediaType.JSON, 
            MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            Func<HttpClient, HttpResponseMessage> httpPost = httpClient => httpClient.PostAsync(uri, postData, GetFormatter(contentType)).Result;
            return ExecuteHttp<TResult>(uri, httpPost, "POST", acceptType, queryParams, headers);
        }

        public static TResult Put<TData, TResult>(string uri, TData postData, MediaType contentType = MediaType.JSON,
            MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            Func<HttpClient, HttpResponseMessage> httpPut = httpClient => httpClient.PutAsync(uri, postData, GetFormatter(contentType)).Result;
            return ExecuteHttp<TResult>(uri, httpPut, "PUT", acceptType, queryParams, headers);
        }

        public static TResult Delete<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            Func<HttpClient, HttpResponseMessage> httpDelete = httpClient => httpClient.DeleteAsync(uri).Result;
            return ExecuteHttp<TResult>(uri, httpDelete, "DELETE", acceptType, queryParams, headers);
        }

        private static MediaTypeFormatter GetFormatter(MediaType format)
        {
            switch (format)
            {
                case MediaType.JSON:
                    return _jsonFormatter;
                case MediaType.XML:
                    return _xmlFormatter;
                case MediaType.ProtoBuf:
                    return _protobufFormatter;
                default:
                    return _jsonFormatter;
            }
        }

        private static IEnumerable<MediaTypeFormatter> GetFormatterAsEnumerable(MediaType format)
        {
            yield return GetFormatter(format);
        }

        private static TResult ExecuteHttp<TResult>(string uri, Func<HttpClient, HttpResponseMessage> httpCall, string httpMethodName,
            MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            if (queryParams != null)
            {
                uri += uri.Contains("?") ? "&" : "?";
                uri += string.Join("&", queryParams.Select(queryParam => queryParam.Key + "=" + queryParam.Value));
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

                using (HttpResponseMessage response = httpCall.Invoke(client))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("HTTP {0} call failed to {1}. Http code: {2}, Reason: {3}", 
                        httpMethodName, uri, response.StatusCode, response.ReasonPhrase));
                }
            }
        }
    }
}