using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Library.Interface;

namespace Template.Library.Service
{
    public class HttpService : IHttpService
    {
        private readonly string _baseUrl;

        public HttpService()
        {
            _baseUrl = "http://localhost/zivoservice.co.zw";
        }
        public T HttpGet<T>(string url, string accessToken = "")
        {
            try
            {
                RestClient _restClient = new RestClient(baseUrl: _baseUrl);

                _restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

                var _request = new RestRequest(resource: url, method: Method.Post);

                var response = _restClient.Get<T>(_request);

                if (response != null) return response;

                throw new Exception("request response was null");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> HttpGetAsync<T>(string url, string accessToken = "")
        {
            try
            {
                RestClient _restClient = new RestClient(baseUrl: _baseUrl);

                _restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

                var _request = new RestRequest(resource: url, method: Method.Post);

                var response = await _restClient.GetAsync<T>(_request);

                if (response != null) return response;

                throw new Exception("request response was null");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public T HttpPost<T>(string url, object model, string accessToken = "")
        {

            try
            {
                var json = JsonSerializer.Serialize(model);

                RestClient _restClient = new RestClient(baseUrl: _baseUrl);

                _restClient.AddDefaultHeader("bearer", accessToken);

                var _request = new RestRequest(resource: url, method: Method.Post);

                _request.AddJsonBody(json);

                var response = _restClient.Execute(_request);

                if (response.IsSuccessful)
                {
                    if (response.Content != null)
                    {
                        var model1 = JsonSerializer.Deserialize<T>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (model1 == null) throw new Exception(response.Content);

                        return model1;
                    }

                    throw new Exception(response.Content);
                }
                else
                {
                    throw new Exception(response.Content);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> HttpPostAsync<T>(string url, object model, string accessToken = "")
        {

            try
            {
                var json = JsonSerializer.Serialize(model);

                RestClient _restClient = new RestClient(baseUrl: _baseUrl);

                _restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

                var _request = new RestRequest(resource: url, method: Method.Post);

                _request.AddJsonBody(json);

                var response = await _restClient.ExecuteAsync(_request);

                if (response.IsSuccessful)
                {
                    if (response.Content != null)
                    {

                        var model1 = JsonSerializer.Deserialize<T>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (model1 == null) throw new Exception(response.Content);

                        return model1;
                    }

                    throw new Exception(response.Content);
                }
                else
                {
                    throw new Exception(response.Content);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
