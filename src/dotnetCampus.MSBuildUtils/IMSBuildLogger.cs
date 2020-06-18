using System.Runtime.CompilerServices;

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
        /// <param name="filePath">自动传入或手工指定，要报告警告的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void Warning(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0);

        /// <summary>
        /// 报告编译错误。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void Error(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0);

        /// <summary>
        /// 立即抛出异常。除报告编译错误外，立刻停止后续的编译过程。
        /// 如果希望输出此编译错误，请全局捕获异常，然后调用 <see cref="MSBuildException.ReportBuildError"/> 方法。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void Throw(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 0);

        /// <summary>
        /// 输出消息到 MSBuild 控制台。
        /// </summary>
        /// <param name="message">编译消息。</param>
        void Output(MSBuildMessage message);
    }
}
