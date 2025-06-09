using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoStudioApp.Service
{
    public class WorkerPhotoWorks : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/workerworks";

        public async Task<List<ImagePhotograph>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<ImagePhotograph>>(BaseUrl);
        }
        public async Task<List<ImagePhotograph>> GetByWorkerId(int id)
        {
            var url = $"{BaseUrl}/byWorker/{id}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await httpClient.GetFromJsonAsync<List<ImagePhotograph>>(BaseUrl);
            }
            return null;
        }

        public async Task<ImagePhotograph> GetById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<ImagePhotograph>();
            }
            return null;
        }
       
        public async Task<int> Create(ImagePhotographDTo imagePhotographDTo)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, imagePhotographDTo);
            if(res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<int>();
            }
            MessageBox.Show(res.ReasonPhrase);
            return -1;
        }
        public async Task<bool> Update(ImagePhotographDTo imagePhotographDTo)
        {
            var url = $"{BaseUrl}/{imagePhotographDTo.Id}";
            var res = await httpClient.PatchAsJsonAsync(url, imagePhotographDTo);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            MessageBox.Show(res.ReasonPhrase);
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var res = await httpClient.DeleteAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
