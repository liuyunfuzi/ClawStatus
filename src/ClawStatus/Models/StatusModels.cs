namespace ClawStatus.Models
{
    /// <summary>
    /// OpenClaw 系统状态
    /// </summary>
    public class SystemStatus
    {
        /// <summary>
        /// 活跃 Agent 数量
        /// </summary>
        public int ActiveAgents { get; set; }

        /// <summary>
        /// 总会话数
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// 系统是否健康
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Token 使用量统计
    /// </summary>
    public class TokenUsage
    {
        /// <summary>
        /// 总 Token 数
        /// </summary>
        public long TotalTokens { get; set; }

        /// <summary>
        /// 输入 Token 数
        /// </summary>
        public long InputTokens { get; set; }

        /// <summary>
        /// 输出 Token 数
        /// </summary>
        public long OutputTokens { get; set; }

        /// <summary>
        /// 时间范围
        /// </summary>
        public string Period { get; set; } = string.Empty;
    }

    /// <summary>
    /// 会话信息
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// 会话 ID
        /// </summary>
        public string SessionKey { get; set; } = string.Empty;

        /// <summary>
        /// 会话类型
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// 是否活跃
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActive { get; set; }
    }
}
