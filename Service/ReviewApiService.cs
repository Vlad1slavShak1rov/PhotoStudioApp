using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class ReviewApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/review";
        public async Task<List<Review>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<Review>>(BaseUrl);
        }
        public async Task<Review> GetById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            return await httpClient.GetFromJsonAsync<Review>(url);
        }
        public async Task<int> Create(ReviewDTO bookingServiceDTO)
        {
            var res = await httpClient.PatchAsJsonAsync(BaseUrl, bookingServiceDTO);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var content = await res.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            return content["id"];
        }
        public async Task<bool> Update(ReviewDTO bookingServiceDTO)
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
