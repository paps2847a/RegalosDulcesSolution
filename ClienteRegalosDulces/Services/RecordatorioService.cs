using Client.Infrastructure;
using DataModel.Models;
using DataModel.Tables;
using System.Text;
using System.Text.Json;

namespace ClienteRegalosDulces.Services
{
    public interface IRecordatorioService
    {
        Task<Recordatorio> Add(Recordatorio row);
        Task<Recordatorio> Update(Recordatorio row);
        Task<bool> Delete(Recordatorio row);
        Task<Recordatorio> GetItem(int Id);
        Task<int> Count();
        Task<IEnumerable<Recordatorio>> GetAllAsync(Filter param);
        Task<Responses> SendMsgTest(Filter param);
    }

    public class RecordatorioService : IRecordatorioService
    {
        private readonly HttpClient _httpClient;
        private Responses _resp;
        private readonly IConfiguration _configuration;

        public RecordatorioService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _resp = new Responses();
            _configuration = configuration;
        }

        public async Task<Responses> SendMsgTest(Filter param)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.SendTestMsg(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
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

        public async Task<Recordatorio> Add(Recordatorio row)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.AddItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Recordatorio();

                if (string.IsNullOrEmpty(responseContent))
                    return new Recordatorio();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Recordatorio>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Recordatorio();
            }
            catch (Exception ex)
            {
                return new Recordatorio();
            }
        }

        public async Task<Recordatorio> Update(Recordatorio row)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.UpdateItem(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(row), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new Recordatorio();

                if (string.IsNullOrEmpty(responseContent))
                    return new Recordatorio();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ?
                    JsonSerializer.Deserialize<Recordatorio>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Recordatorio();
            }
            catch (Exception ex)
            {
                return new Recordatorio();
            }
        }

        public async Task<bool> Delete(Recordatorio row)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.UpdateItem(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<Recordatorio> GetItem(int Id)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.GetItem(_configuration["ApiBaseUrl"].ToString(), Id);
                var response = await _httpClient.GetAsync(uri);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new Recordatorio();
                
                if (string.IsNullOrEmpty(responseContent))
                    return new Recordatorio();
                
                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                
                return _resp.Success ? JsonSerializer.Deserialize<Recordatorio>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : new Recordatorio();
            }
            catch (Exception ex)
            {
                return new Recordatorio();
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string uri = ApiUrl.Recordatorio.Count(_configuration["ApiBaseUrl"].ToString());
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

        public async Task<IEnumerable<Recordatorio>> GetAllAsync(Filter param)
        {
            try
            {
                string uri = ApiUrl.Recordatorio.Get(_configuration["ApiBaseUrl"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Recordatorio>();

                if (string.IsNullOrEmpty(responseContent))
                    return Enumerable.Empty<Recordatorio>();

                _resp = JsonSerializer.Deserialize<Responses>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

                return _resp.Success ? 
                    JsonSerializer.Deserialize<IEnumerable<Recordatorio>>(_resp.Data.ToString(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) : Enumerable.Empty<Recordatorio>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Recordatorio>();
            }
        }
    }
}
