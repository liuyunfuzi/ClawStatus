using System.Net.Http;
using System.Text.Json;

namespace ClawStatus.Services
{
    /// <summary>
    /// 状态服务实现 - 调用 OpenClaw API 获取状态
    /// </summary>
    public class StatusService : IStatusService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private string _apiBaseUrl = "http://localhost:8080"; // 默认 API 地址
        private bool _isInitialized;

        public StatusService()
        {
            _httpClient = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task InitializeAsync()
        {
            // TODO: 从配置文件读取 API 地址
            // 可以尝试从环境变量或配置文件中读取
            var apiUrl = Environment.GetEnvironmentVariable("CLAWSTATUS_API_URL");
            if (!string.IsNullOrEmpty(apiUrl))
            {
                _apiBaseUrl = apiUrl;
            }

            _isInitialized = true;
            await Task.CompletedTask;
        }

        public async Task<AgentStatus> GetStatusAsync()
        {
            if (!_isInitialized)
            {
                return AgentStatus.Offline;
            }

            try
            {
                // 调用 API 获取状态
                var activeCount = await GetActiveAgentCountAsync();
                
                if (activeCount == 0)
                {
                    return AgentStatus.Offline;
                }
                else if (activeCount >= 5)
                {
                    return AgentStatus.Heavy;
                }
                else if (activeCount >= 2)
                {
                    return AgentStatus.Busy;
                }
                else
                {
                    return AgentStatus.Idle;
                }
            }
            catch (Exception)
            {
                // API 调用失败时返回错误状态
                return AgentStatus.Error;
            }
        }

        public async Task<int> GetActiveAgentCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/status");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("activeAgents", out var activeAgentsElement))
                    {
                        return activeAgentsElement.GetInt32();
                    }
                }
            }
            catch (Exception)
            {
                // 静默失败，返回 0
            }
            return 0;
        }

        public async Task<long> GetTodayTokenUsageAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/usage/today");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("totalTokens", out var totalTokensElement))
                    {
                        return totalTokensElement.GetInt64();
                    }
                }
            }
            catch (Exception)
            {
                // 静默失败，返回 0
            }
            return 0;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
