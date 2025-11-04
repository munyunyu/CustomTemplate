using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Models;

namespace Template.Library.Interface
{
    public interface IHttpService
    {
        T HttpGet<T>(string url, string accessToken = "") where T : IResponse, new();
        Task<T> HttpGetAsync<T>(string url, string accessToken = "") where T : IResponse, new();
        T HttpPost<T>(string url, object model, string accessToken = "") where T : IResponse, new();
        Task<T> HttpPostAsync<T>(string url, object model, string accessToken = "") where T : IResponse, new();
    }
}
