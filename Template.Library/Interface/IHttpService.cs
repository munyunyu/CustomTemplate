using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Interface
{
    public interface IHttpService
    {
        T HttpGet<T>(string url, string accessToken = "");
        Task<T> HttpGetAsync<T>(string url, string accessToken = "");
        T HttpPost<T>(string url, object model, string accessToken = "");
        Task<T> HttpPostAsync<T>(string url, object model, string accessToken = "");
    }
}
