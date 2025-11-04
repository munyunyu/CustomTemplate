using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Library.Enums;
using Template.Library.Interface;
using Template.Library.Models;
using Template.Library.Models.POCO;

namespace Template.Library.Service
{
    public class HttpService : IHttpService
    {
        private readonly string _baseUrl;

        public HttpService(IOptions<ApplicationSettings> options)
        {
            _baseUrl = options.Value.ApiBaseUrl;
        }

        public T HttpGet<T>(string url, string accessToken = "") where T : IResponse, new()
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
            catch (HttpRequestException ex)
            {
                return new T { Code = Status.Failed, Message = $"Network error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new T { Code = Status.Failed, Message = $"Unexpected error: {ex.Message}" };
            }
        }

        public async Task<T> HttpGetAsync<T>(string url, string accessToken = "") where T : IResponse, new()
        {
            try
            {
                RestClient _restClient = new RestClient(baseUrl: _baseUrl);

                _restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");

                var _request = new RestRequest(resource: url, method: Method.Get);

                var response = await _restClient.ExecuteAsync(_request);

                if (response.IsSuccessful)
                {
                    if (string.IsNullOrEmpty(response.Content)) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    var model1 = JsonSerializer.Deserialize<T>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (model1 == null) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    return model1;
                }
                else
                {
                    return new T { Code = Status.Failed, Message = response.StatusDescription };
                }

            }
            catch (HttpRequestException ex)
            {
                return new T { Code = Status.Failed, Message = $"Network error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new T { Code = Status.Failed, Message = $"Unexpected error: {ex.Message}" };
            }
        }

        public T HttpPost<T>(string url, object model, string accessToken = "") where T : IResponse, new()
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
                    if (string.IsNullOrEmpty(response.Content)) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    var model1 = JsonSerializer.Deserialize<T>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (model1 == null) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    return model1;
                    
                }
                else
                {
                    return new T { Code = Status.Failed, Message = response.StatusDescription };
                }

            }
            catch (HttpRequestException ex)
            {
                return new T { Code = Status.Failed, Message = $"Network error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new T { Code = Status.Failed, Message = $"Unexpected error: {ex.Message}" };
            }
        }

        public async Task<T> HttpPostAsync<T>(string url, object model, string accessToken = "") where T : IResponse, new()
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
                    if (string.IsNullOrEmpty(response.Content)) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    var model1 = JsonSerializer.Deserialize<T>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (model1 == null) return new T { Code = Status.SuccessWithWarning, Message = response.Content };

                    return model1;
                }
                else
                {
                    return new T { Code = Status.Failed, Message = response.StatusDescription };
                }

            }
            catch (HttpRequestException ex)
            {
                return new T { Code = Status.Failed, Message = $"Network error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new T { Code = Status.Failed, Message = $"Unexpected error: {ex.Message}" };
            }
        }
    }
}
