using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface IWsGroupService
    {
        Task<WsGroup> Add(WsGroup row);
        Task<WsGroup> Update(WsGroup row);
        Task<bool> Delete(WsGroup row);
        Task<WsGroup> GetItem(int Id);
        Task<Responses> SyncGroups();
        Task<int> Count();
        Task<IEnumerable<WsGroup>> GetAllAsync(Filter param);
    }

    public class WsGroupService : IWsGroupService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public WsGroupService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Responses> SyncGroups()
        {
            try
            {
                string uri = ApiUrl.WsGroup.Sync(_configuration["ApiBaseUrl"].ToString());
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Responses
                    {
                        Success = false,
                        Message = "Error al enviar el mensaje de prueba."
                    };

                if (string.IsNullOrEmpty(responseContent))
                    return new Responses
                    {
                        Success = false,
                        Message = "Error al enviar el mensaje de prueba."
                    };

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? new Responses() { Message = "Mensaje de Prueba enviado" } : new Responses
                {
                    Success = false,
                    Message = "Error al enviar el mensaje de prueba."
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    Success = false,
                    Message = "Error al enviar el mensaje de prueba."
                };
            }
        }

        public async Task<WsGroup> Add(WsGroup row)
        {
            try
            {
                string uri = ApiUrl.WsGroup.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new WsGroup();

                if (string.IsNullOrEmpty(responseContent))
                    return new WsGroup();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<WsGroup>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new WsGroup();
            }
            catch (Exception ex)
            {
                return new WsGroup();
            }
        }

        public async Task<WsGroup> Update(WsGroup row)
        {
            try
            {
                string uri = ApiUrl.WsGroup.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new WsGroup();

                if (string.IsNullOrEmpty(responseContent))
                    return new WsGroup();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<WsGroup>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new WsGroup();
            }
            catch (Exception ex)
            {
                return new WsGroup();
            }
        }

        public async Task<bool> Delete(WsGroup row)
        {
            try
            {
                string uri = ApiUrl.WsGroup.DeleteItem(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<WsGroup> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.WsGroup.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new WsGroup();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new WsGroup();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<WsGroup>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new WsGroup();
            }
            catch (Exception ex)
            {
                return new WsGroup();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.WsGroup.Count(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<IEnumerable<WsGroup>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.WsGroup.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<WsGroup>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<WsGroup>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<WsGroup>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<WsGroup>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<WsGroup>();
            }
        }
    }
}
