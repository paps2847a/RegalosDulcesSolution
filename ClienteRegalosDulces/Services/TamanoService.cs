using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface ITamanoService
    {
        Task<Tamano> Add(Tamano row);
        Task<Tamano> Update(Tamano row);
        Task<bool> Delete(Tamano row);
        Task<Tamano> GetItem(int Id);
        Task<int> Count();
        Task<IEnumerable<Tamano>> GetAllAsync(Filter param);
    }

    public class TamanoService : ITamanoService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public TamanoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Tamano> Add(Tamano row)
        {
            try
            {
                string uri = ApiUrl.Tamano.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Tamano();

                if (string.IsNullOrEmpty(responseContent))
                    return new Tamano();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Tamano>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Tamano();
            }
            catch (Exception ex)
            {
                return new Tamano();
            }
        }

        public async Task<Tamano> Update(Tamano row)
        {
            try
            {
                string uri = ApiUrl.Tamano.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Tamano();

                if (string.IsNullOrEmpty(responseContent))
                    return new Tamano();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Tamano>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Tamano();
            }
            catch (Exception ex)
            {
                return new Tamano();
            }
        }

        public async Task<bool> Delete(Tamano row)
        {
            try
            {
                string uri = ApiUrl.Tamano.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return false;

                if (string.IsNullOrEmpty(responseContent))
                    return false;

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Tamano> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.Tamano.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new Tamano();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new Tamano();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<Tamano>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Tamano();
            }
            catch (Exception ex)
            {
                return new Tamano();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.Tamano.Count(_configuration["ApiBaseUrl"].ToString());
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return 0;

                if (string.IsNullOrEmpty(responseContent))
                    return 0;

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                return _resp.Success ? JsonSerializer.Deserialize<int>(_resp.Data.ToString()) : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<Tamano>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.Tamano.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Tamano>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<Tamano>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<Tamano>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<Tamano>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Tamano>();
            }
        }
    }
}
