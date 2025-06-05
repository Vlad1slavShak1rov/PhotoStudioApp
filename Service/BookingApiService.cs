using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class BookingApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/booking";

        public async Task<List<Booking>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<Booking>>(BaseUrl);
        }
        public async Task<Booking> GetById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            return await httpClient.GetFromJsonAsync<Booking>(url);
        }
        public async Task<int> Create(BookingServiceDTO bookingServiceDTO)
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
        public async Task<bool> Update(BookingServiceDTO bookingServiceDTO)
        {
            var url = $"{BaseUrl}/{bookingServiceDTO.ID}";
            var res = await httpClient.PatchAsJsonAsync(url,bookingServiceDTO);
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
            if(!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return false;
            }
            return true;
        }
    }
}
