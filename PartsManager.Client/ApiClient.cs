using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using PartsManager.Shared.DTOs;

namespace PartsManager.Client
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string baseUrl, int timeoutSeconds = 30)
        {
            // 強制設定 TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<MaterialStockInfoDto> GetInventoryAsync(string barcode)
        {
            var response = await _client.GetAsync("api/inventory/scan/" + barcode);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MaterialStockInfoDto>(json);
            }
            
            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("API Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<bool> PostInboundAsync(InboundDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/inventory/inbound", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("Inbound Error: " + errorJson);
        }

        public async Task<OutboundResultDto> PostOutboundAsync(OutboundDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/inventory/outbound", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<OutboundResultDto>(responseJson);
            }

            try
            {
                return JsonConvert.DeserializeObject<OutboundResultDto>(responseJson);
            }
            catch
            {
                throw new Exception("Outbound Error: " + response.StatusCode + " " + responseJson);
            }
        }

        public async Task<List<LowStockDto>> GetLowStockAsync()
        {
            var response = await _client.GetAsync("api/inventory/low-stock");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<LowStockDto>>(json);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("GetLowStock Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<List<SparePartSearchResultDto>> SearchInventoryAsync(string keyword)
        {
            keyword = keyword ?? "";
            // 注意：API 端的參數名稱是 query
            var url = "api/inventory/search?query=" + Uri.EscapeDataString(keyword);

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SparePartSearchResultDto>>(json);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("Search Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<List<WarehouseDto>> GetWarehousesAsync()
        {
            var response = await _client.GetAsync("api/masterdata/warehouses");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<WarehouseDto>>(json);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("GetWarehouses Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<MaterialDto> CreateMaterialAsync(CreateMaterialDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/masterdata/materials", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MaterialDto>(responseJson);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("CreateMaterial Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<MaterialDto> GetMaterialAsync(int id)
        {
            var response = await _client.GetAsync("api/masterdata/materials/" + id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MaterialDto>(json);
            }
            return null;
        }

        public async Task UpdateMaterialAsync(int id, UpdateMaterialDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/masterdata/materials/" + id, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new Exception("UpdateMaterial Error: " + response.StatusCode + " " + errorJson);
            }
        }

        public async Task DeleteMaterialAsync(int id)
        {
            var response = await _client.DeleteAsync("api/masterdata/materials/" + id);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new Exception("DeleteMaterial Error: " + response.StatusCode + " " + errorJson);
            }
        }

        public async Task UploadAttachmentsAsync(int id, List<string> filePaths)
        {
            using (var content = new MultipartFormDataContent())
            {
                foreach (var path in filePaths)
                {
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(path));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
                    content.Add(fileContent, "files", System.IO.Path.GetFileName(path));
                }

                var response = await _client.PostAsync($"api/masterdata/materials/{id}/attachments", content);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    throw new Exception("Upload Failed: " + err);
                }
            }
        }

        public async Task<List<AttachmentDto>> GetAttachmentsAsync(int materialId)
        {
            var response = await _client.GetAsync($"api/masterdata/materials/{materialId}/attachments");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AttachmentDto>>(json);
            }
            return new List<AttachmentDto>();
        }

        public async Task<byte[]> DownloadAttachmentAsync(int materialId, string fileName)
        {
            var response = await _client.GetAsync($"api/masterdata/materials/{materialId}/attachments/{Uri.EscapeDataString(fileName)}/download");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }

        public async Task DeleteAttachmentAsync(int materialId, string fileName)
        {
            var response = await _client.DeleteAsync($"api/masterdata/materials/{materialId}/attachments/{Uri.EscapeDataString(fileName)}");
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new Exception("Delete Attachment Failed: " + err);
            }
        }

        public async Task<MaterialStockInfoDto> GetScanResultAsync(string barcode)
        {
            var response = await _client.GetAsync("api/inventory/scan/" + barcode);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MaterialStockInfoDto>(json);
            }
            
            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("Scan Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<List<InventoryComparisonResultDto>> CompareInventoryAsync(InventoryCheckRequestDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/inventory/compare", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<InventoryComparisonResultDto>>(responseJson);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("Compare Error: " + response.StatusCode + " " + errorJson);
        }

        // --- Auth & User Management ---

        public async Task<UserDto> LoginAsync(string username, string password)
        {
            var data = new LoginDto { Username = username, Password = password };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var resJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDto>(resJson);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception("Login Error: " + response.StatusCode + " " + errorJson);
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _client.GetAsync("api/user");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserDto>>(json);
            }
            throw new Exception("無法取得使用者列表");
        }

        public async Task CreateUserAsync(CreateUserDto data, int requesterLevel)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Remove("X-User-Level");
            _client.DefaultRequestHeaders.Add("X-User-Level", requesterLevel.ToString());

            var response = await _client.PostAsync("api/user", content);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new Exception("建立使用者失敗: " + err);
            }
        }

        public async Task DeleteUserAsync(int id, int requesterLevel)
        {
            _client.DefaultRequestHeaders.Remove("X-User-Level");
            _client.DefaultRequestHeaders.Add("X-User-Level", requesterLevel.ToString());

            var response = await _client.DeleteAsync("api/user/" + id);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new Exception("刪除使用者失敗: " + err);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/user/change-password", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new Exception("變更密碼失敗: " + errorJson);
            }
        }
    }
}
