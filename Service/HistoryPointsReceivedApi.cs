using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Service
{
    public class HistoryPointsReceivedApi : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/historypoints";

        public async Task<List<HistoryPointsReceived>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<HistoryPointsReceived>>(BaseUrl);
        }
        public async Task<HistoryPointsReceived> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            return await httpClient.GetFromJsonAsync<HistoryPointsReceived>(url);
        }
        public async Task<List<HistoryPointsReceived>> GetByCustomerId(int userId)
        {
            var url = $"{BaseUrl}/byUser/{userId}";
            return await httpClient.GetFromJsonAsync<List<HistoryPointsReceived>>(url);
        }
        public async Task<int> Create(HistoryPointsReceivedDTO bookingServiceDTO)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, bookingServiceDTO);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var content = await res.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            return content["id"];
        }
        public async Task<bool> Update(HistoryPointsReceivedDTO bookingServiceDTO)
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
