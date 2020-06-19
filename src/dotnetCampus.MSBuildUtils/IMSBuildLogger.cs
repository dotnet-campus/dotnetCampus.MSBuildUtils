namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 为项目提供输出到 MSBuild 控制台的日志机制。
    /// </summary>
    internal interface IMSBuildLogger
    {
        /// <summary>
        /// 输出编译过程信息。
        /// </summary>
        /// <param name="message">任何种类的信息。</param>
        void Message(string message);

        /// <summary>
        /// 报告编译警告。
        /// </summary>
        /// <param name="message">编译警告消息。</param>
        /// <param name="id">编译提示/警告/错误的唯一标识符。例如 CS8601，前面使用 2~4 个全大写字母表示编译器代号，后面使用 2~4 个数字表示特定编译警告或错误的编号。</param>
        /// <param name="targetFile">报告的让 IDE 跳转到的文件。</param>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        void Warning(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null);

        /// <summary>
        /// 报告编译错误。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="id">编译提示/警告/错误的唯一标识符。例如 CS8601，前面使用 2~4 个全大写字母表示编译器代号，后面使用 2~4 个数字表示特定编译警告或错误的编号。</param>
        /// <param name="targetFile">报告的让 IDE 跳转到的文件。</param>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        void Error(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null);

        /// <summary>
        /// 立即抛出异常。除报告编译错误外，立刻停止后续的编译过程。
        /// 如果希望输出此编译错误，请全局捕获异常，然后调用 <see cref="MSBuildException.ReportBuildError"/> 方法。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="targetFile">报告的让 IDE 跳转到的文件。</param>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        void Throw(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null);
    }
}
