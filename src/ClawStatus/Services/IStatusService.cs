namespace ClawStatus.Services
{
    /// <summary>
    /// 状态服务接口
    /// </summary>
    public interface IStatusService : IDisposable
    {
        /// <summary>
        /// 初始化服务
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取当前状态
        /// </summary>
        Task<AgentStatus> GetStatusAsync();

        /// <summary>
        /// 获取活跃 Agent 数量
        /// </summary>
        Task<int> GetActiveAgentCountAsync();

        /// <summary>
        /// 获取今日 Token 使用量
        /// </summary>
        Task<long> GetTodayTokenUsageAsync();
    }
}
