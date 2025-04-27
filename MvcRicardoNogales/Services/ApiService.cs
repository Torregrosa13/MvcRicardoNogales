using Newtonsoft.Json;
using System.Text;

namespace MvcRicardoNogales.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Método genérico para hacer GET
        public async Task<T> GetAsync<T>(string url, string token = null)
        {
            var client = _httpClientFactory.CreateClient("ApiMaraton");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        // Método genérico para hacer POST
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, string token = null)
        {
            var client = _httpClientFactory.CreateClient("ApiMaraton");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // 🔥 Serializar y mostrar el JSON que vamos a enviar
            var jsonData = JsonConvert.SerializeObject(data);
            Console.WriteLine("JSON que se envía: " + jsonData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

    }
}
