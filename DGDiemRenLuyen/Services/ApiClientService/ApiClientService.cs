using DGDiemRenLuyen.DTOs.Responses;

namespace DGDiemRenLuyen.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<T>> GetDataFromApi<T>(string url, object requestData = null)
        {
            HttpResponseMessage response;

            // Kiểm tra nếu có dữ liệu yêu cầu (requestData), dùng phương thức POST
            if (requestData != null)
            {
                response = await _httpClient.PostAsJsonAsync(url, requestData);
            }
            else
            {
                response = await _httpClient.GetAsync(url);
            }

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                return apiResponse;
            }

            return null;
        }

        public async Task<ApiResponse<T>> GetDataFromApi<T>(string url)
        {
            HttpResponseMessage response;

            response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                return apiResponse;
            }

            return null;
        }
    }
}
