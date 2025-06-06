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
    public class CustomerApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/customer";

        public async Task<List<Customer>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<Customer>>(BaseUrl);
        }
        public async Task<Customer> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            return await httpClient.GetFromJsonAsync<Customer>(url);
        }
        public async Task<Customer> GetByUserId(int userId)
        {
            var url = $"{BaseUrl}/byUser/{userId}";

            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<Customer>();
            }
            else return null;
        }
        public async Task<int> Create(CustomerDTO bookingServiceDTO)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, bookingServiceDTO);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var content = await res.Content.ReadFromJsonAsync<Customer>();
            return content.ID;
        }
        public async Task<bool> Update(CustomerDTO bookingServiceDTO)
        {
            var url = $"{BaseUrl}/{bookingServiceDTO.ID}";
            var res = await httpClient.PatchAsJsonAsync(url, bookingServiceDTO);
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
