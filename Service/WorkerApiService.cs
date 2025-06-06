using Azure;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class WorkerApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/worker";
        public async Task<List<Worker>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<Worker>>(BaseUrl);
        }
        public async Task<List<Worker>> GetAllVisagiste()
        {
            var url = $"{BaseUrl}/visagiste";
            return await httpClient.GetFromJsonAsync<List<Worker>>(url);
        }
        public async Task<List<Worker>> GetAllPhotograph()
        {
            var url = $"{BaseUrl}/photograph";
            return await httpClient.GetFromJsonAsync<List<Worker>>(url);
        }

        public async Task<Worker> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            return await httpClient.GetFromJsonAsync<Worker>(url);
        }
        public async Task<Worker> GetByUserId(int userId)
        {
            var url = $"{BaseUrl}/byUser/{userId}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await httpClient.GetFromJsonAsync<Worker>(url);
            }
            
            if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            res.EnsureSuccessStatusCode();
            return null;
        }
        public async Task<Worker> GetByVisagiste(int id)
        {
            var url = $"{BaseUrl}/visagiste/{id}";
            return await httpClient.GetFromJsonAsync<Worker>(url);
        }
        public async Task<Worker> GetByPhotograph(int id)
        {
            var url = $"{BaseUrl}/photograph/{id}";
            return await httpClient.GetFromJsonAsync<Worker>(url);
        }

        public async Task<int> Create(WorkerDTO entity)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, entity);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var id = await res.Content.ReadFromJsonAsync<int>();
            return id;
        }
        public async Task<bool> Update(WorkerDTO entity)
        {
            var url = $"{BaseUrl}/{entity.ID}";
            var res = await httpClient.PatchAsJsonAsync(url, entity);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var res = await httpClient.DeleteAsync(url);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return false;
            }
            return true;
        }
    }
}
