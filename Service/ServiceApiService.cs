using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class ServiceApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/service";

        public async Task<List<Services>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<Services>>(BaseUrl);
        }
        public async Task<Services> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            return await httpClient.GetFromJsonAsync<Services>(url);
        }
        public async Task<Services> GetByName(string name)
        {
            var url = $"{BaseUrl}/name/{name}";
            return await httpClient.GetFromJsonAsync<Services>(url);
        }
        public async Task<int> Create(ServiceDTO entity)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, entity);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var content = await res.Content.ReadFromJsonAsync<int>();
            return content;
        }
        public async Task<bool> Update(ServiceDTO entity)
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
