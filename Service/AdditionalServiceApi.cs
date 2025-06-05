using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PhotoStudioApp.Service
{
    public class AdditionalServiceApi : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/addservice";

        public async Task<List<AdditionalService>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<AdditionalService>>(BaseUrl);
        }

        public async Task<AdditionalService> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            return await httpClient.GetFromJsonAsync<AdditionalService>(url);
        }
        public async Task<AdditionalService> GetByName(string name)
        {
            var url = $"{BaseUrl}/name/{name}";
            return await httpClient.GetFromJsonAsync<AdditionalService>(url);
        }
        public async Task<int> Create(AdditionalServiceDTO service)
        {
           var res = await httpClient.PostAsJsonAsync(BaseUrl, service);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var content = await res.Content.ReadFromJsonAsync<Dictionary<string,int>>();
            return content["id"];
        }

        public async Task<bool> Update(AdditionalServiceDTO service)
        {
            var url = $"{BaseUrl}/{service.ID}";
            var res = await httpClient.PatchAsJsonAsync(url, service);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase,"Ошибка");
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var res = await httpClient.DeleteAsync(url);
            if(!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return false;
            }
            return true;
        }

    }
}
