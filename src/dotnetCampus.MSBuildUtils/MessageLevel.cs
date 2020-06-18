namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 标识 MSBuild 输出的消息的等级。
    /// </summary>
    internal enum MessageLevel
    {
        /// <summary>
        /// 消息。
        /// </summary>
        Message,

        /// <summary>
        /// 警告。
        /// </summary>
        Warning,
        
        /// <summary>
        /// 错误。
        /// </summary>
        Error,
    }
}
