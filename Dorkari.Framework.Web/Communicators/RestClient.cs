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

        public static TResult Get<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
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
                
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    //var contentLength = response.Content.Headers.ContentLength; //TODO: for test only
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("GET call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }
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

        //TODO: needs more work
        public static TResult Post<TData, TResult>(string uri, TData postData, MediaType contentType = MediaType.JSON, 
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

                using (HttpResponseMessage response = client.PostAsync(uri, postData, GetFormatter(contentType)).Result) //(HttpResponseMessage response = client.PostAsJsonAsync(url, data).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("POST call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }
        }

        public static TResult Put<TData, TResult>(string uri, TData postData, MediaType contentType = MediaType.JSON,
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

                using (HttpResponseMessage response = client.PutAsync(uri, postData, GetFormatter(contentType)).Result) //(HttpResponseMessage response = client.PostAsJsonAsync(url, data).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("PUT call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }
        }

        public static TResult Delete<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
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

                using (HttpResponseMessage response = client.DeleteAsync(uri).Result) //(HttpResponseMessage response = client.PostAsJsonAsync(url, data).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("DELETE call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }
        }

        public static TResult ExecuteHttp<TResult>(string uri, MediaType acceptType = MediaType.JSON, Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null)
        {
            //Func<HttpClient, HttpResponseMessage> httpCall = httpClient => httpClient.DeleteAsync(uri).Result;
            //=============================
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

                using (HttpResponseMessage response = client.DeleteAsync(uri).Result) //(HttpResponseMessage response = client.PostAsJsonAsync(url, data).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<TResult>(GetFormatterAsEnumerable(acceptType)).Result;
                    }
                    throw new Exception(string.Format("DELETE call failed to {0}. Http code: {1}, Reason: {2}", uri, response.StatusCode, response.ReasonPhrase));
                }
            }
        }
    }
}