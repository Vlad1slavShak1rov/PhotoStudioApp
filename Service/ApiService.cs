using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class ApiService
    {
        protected readonly HttpClient httpClient;
        protected virtual string BaseUrl => "https://localhost:7158/api";
        public ApiService()
        {
            httpClient = new HttpClient();
        }
    }
}
