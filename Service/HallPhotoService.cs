using PhotoStudioApp.Model;
using PhotoStudioApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoStudioApp.Service
{
    public class HallPhotoService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl + "/hallphoto";

        public async Task<List<HallPhoto>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<HallPhoto>>(BaseUrl);
        }
        public async Task<HallPhoto> GetById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<HallPhoto>();
            }
            return null;
        }
        public async Task<List<HallPhoto>> GetByHallId(int id)
        {
            var url = $"{BaseUrl}/byHall/{id}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<List<HallPhoto>>();
            }
            return null;
        }

        public async Task<int> Create(HallPhotoDTO hallPhoto)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, hallPhoto);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<int>();
            }
            MessageBox.Show(res.ReasonPhrase);
            return -1;
        }
        public async Task<bool> Update(HallPhotoDTO hallPhoto)
        {
            var url = $"{BaseUrl}/{hallPhoto.Id}";
            var res = await httpClient.PatchAsJsonAsync(url, hallPhoto);
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
