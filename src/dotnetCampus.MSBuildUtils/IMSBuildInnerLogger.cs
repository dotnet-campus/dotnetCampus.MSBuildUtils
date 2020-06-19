using System.Runtime.CompilerServices;

namespace dotnetCampus.MSBuildUtils
{
    internal interface IMSBuildInnerLogger : IMSBuildLogger
    {
        /// <summary>
        /// 报告编译警告，指示编译器内部实现可能有问题。问题将指向编译器内部的源代码和代码行号。
        /// </summary>
        /// <param name="message">编译警告消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告警告的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void InnerWarning(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1);

        /// <summary>
        /// 报告编译错误，指示编译器内部发生了错误。错误将指向编译器内部的源代码和代码行号。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void InnerError(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1);

        /// <summary>
        /// 立即抛出编译器内部异常。除报告编译错误外，立刻停止后续的编译过程。错误将指向编译器内部的源代码和代码行号。
        /// 如果希望输出此编译错误，请全局捕获异常，然后调用 <see cref="MSBuildException.ReportBuildError"/> 方法。
        /// </summary>
        /// <param name="message">编译错误消息。</param>
        /// <param name="filePath">自动传入或手工指定，要报告错误的文件，可用于 Visual Studio 自动定位到目标文件方便调试错误。</param>
        /// <param name="line">自动传入或手工指定，要报告错误的文件的行号，可用于 Visual Studio 自动定位到目标文件的对应位置方便调试错误。</param>
        void InnerThrow(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1);
    }
}
