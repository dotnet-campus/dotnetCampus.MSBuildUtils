using System;
using System.Runtime.CompilerServices;

namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 提供输出到 MSBuild 控制台的基本实现。
    /// </summary>
    internal class MSBuildConsoleLogger : IMSBuildLogger
    {
        /// <inheritdoc/>
        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        /// <inheritdoc/>
        public void Warning(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
            => Output(new MSBuildMessage(message, MessageLevel.Warning, null, filePath, line));

        /// <inheritdoc/>
        public void Error(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
            => Output(new MSBuildMessage(message, MessageLevel.Error, null, filePath, line));

        /// <inheritdoc/>
        public void Throw(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
        {
            throw new MSBuildException(new MSBuildMessage(message, MessageLevel.Error, null, filePath, line));
        }

        /// <inheritdoc/>
        public void Output(MSBuildMessage message) => Console.WriteLine(message);
    }
}
