using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface IMensajeService
    {
        Task<Mensaje> Add(Mensaje row);
        Task<Mensaje> Update(Mensaje row);
        Task<bool> Delete(Mensaje row);
        Task<Mensaje> GetItem(int Id);
        Task<int> Count();
        Task<IEnumerable<Mensaje>> GetAllAsync(Filter param);
    }

    public class MensajeService : IMensajeService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public MensajeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Mensaje> Add(Mensaje row)
        {
            try
            {
                string uri = ApiUrl.Mensaje.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Mensaje();

                if (string.IsNullOrEmpty(responseContent))
                    return new Mensaje();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Mensaje>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Mensaje();
            }
            catch (Exception ex)
            {
                return new Mensaje();
            }
        }

        public async Task<Mensaje> Update(Mensaje row)
        {
            try
            {
                string uri = ApiUrl.Mensaje.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Mensaje();

                if (string.IsNullOrEmpty(responseContent))
                    return new Mensaje();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Mensaje>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Mensaje();
            }
            catch (Exception ex)
            {
                return new Mensaje();
            }
        }

        public async Task<bool> Delete(Mensaje row)
        {
            try
            {
                string uri = ApiUrl.Mensaje.DeleteItem(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<Mensaje> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.Mensaje.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new Mensaje();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new Mensaje();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<Mensaje>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Mensaje();
            }
            catch (Exception ex)
            {
                return new Mensaje();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.Mensaje.Count(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<IEnumerable<Mensaje>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.Mensaje.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Mensaje>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<Mensaje>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<Mensaje>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<Mensaje>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Mensaje>();
            }
        }
    }
}
