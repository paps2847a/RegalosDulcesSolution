using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface ICategoriaService
    {
        Task<Categoria> Add(Categoria row);
        Task<Categoria> Update(Categoria row);
        Task<bool> Delete(Categoria row);
        Task<Categoria> GetItem(int Id);
        Task<int> Count();
        Task<IEnumerable<Categoria>> GetAllAsync(Filter param);
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public CategoriaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Categoria> Add(Categoria row)
        {
            try
            {
                string uri = ApiUrl.Categoria.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Categoria();

                if (string.IsNullOrEmpty(responseContent))
                    return new Categoria();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Categoria>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Categoria();
            }
            catch (Exception ex)
            {
                return new Categoria();
            }
        }

        public async Task<Categoria> Update(Categoria row)
        {
            try
            {
                string uri = ApiUrl.Categoria.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Categoria();

                if (string.IsNullOrEmpty(responseContent))
                    return new Categoria();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Categoria>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Categoria();
            }
            catch (Exception ex)
            {
                return new Categoria();
            }
        }

        public async Task<bool> Delete(Categoria row)
        {
            try
            {
                string uri = ApiUrl.Categoria.DeleteItem(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<Categoria> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.Categoria.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new Categoria();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new Categoria();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<Categoria>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Categoria();
            }
            catch (Exception ex)
            {
                return new Categoria();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.Categoria.Count(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<IEnumerable<Categoria>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.Categoria.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Categoria>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<Categoria>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<Categoria>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<Categoria>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Categoria>();
            }
        }
    }
}
