using OfficeOpenXml.FormulaParsing.FormulaExpressions.CompileResults;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoStudioApp.Service
{
    public class UserApiService : ApiService
    {
        protected override string BaseUrl => base.BaseUrl+"/user";
        public async Task<List<User>> GetAll()
        {
            return await httpClient.GetFromJsonAsync<List<User>>(BaseUrl);
        }
        public async Task<User> GetById(int id)
        {
            var url = $"{BaseUrl}/id/{id}";
            var res = await httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                return await httpClient.GetFromJsonAsync<User>(url);
            }
            return null;
        }
        public async Task<User> GetByLogin(string login)
        {
            var url = $"{BaseUrl}/login/{login}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return null;
        }
        public async Task<int> Create(UserDTO entity)
        {
            var res = await httpClient.PostAsJsonAsync(BaseUrl, entity);
            if (!res.IsSuccessStatusCode)
            {
                System.Windows.MessageBox.Show(res.ReasonPhrase, "Ошибка");
                return -1;
            }

            var user = await res.Content.ReadFromJsonAsync<User>();
            return user.ID;
        }
        public async Task<bool> Update(UserDTO entity)
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
