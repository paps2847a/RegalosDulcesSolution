using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface IInventarioService
    {
        Task<Inventario> Add(Inventario row);
        Task<Inventario> Update(Inventario row);
        Task<bool> Delete(Inventario row);
        Task<Inventario> GetItem(int Id);
        Task<int> Count();
        Task<IEnumerable<Inventario>> GetAllAsync(Filter param);
    }

    public class InventarioService : IInventarioService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public InventarioService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Inventario> Add(Inventario row)
        {
            try
            {
                string uri = ApiUrl.Inventario.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Inventario();

                if (string.IsNullOrEmpty(responseContent))
                    return new Inventario();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Inventario>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Inventario();
            }
            catch (Exception ex)
            {
                return new Inventario();
            }
        }

        public async Task<Inventario> Update(Inventario row)
        {
            try
            {
                string uri = ApiUrl.Inventario.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Inventario();

                if (string.IsNullOrEmpty(responseContent))
                    return new Inventario();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Inventario>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Inventario();
            }
            catch (Exception ex)
            {
                return new Inventario();
            }
        }

        public async Task<bool> Delete(Inventario row)
        {
            try
            {
                string uri = ApiUrl.Inventario.DeleteItem(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<Inventario> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.Inventario.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new Inventario();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new Inventario();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<Inventario>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Inventario();
            }
            catch (Exception ex)
            {
                return new Inventario();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.Inventario.Count(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<IEnumerable<Inventario>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.Inventario.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Inventario>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<Inventario>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<Inventario>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<Inventario>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Inventario>();
            }
        }
    }
}
