using System;
using System.Runtime.CompilerServices;

namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 提供输出到 MSBuild 控制台的基本实现。
    /// </summary>
    internal class MSBuildConsoleLogger : IMSBuildLogger, IMSBuildInnerLogger
    {
        /// <inheritdoc/>
        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        /// <inheritdoc/>
        public void InnerWarning(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
            => new MSBuildMessage(message, null, filePath, line).Warning();

        /// <inheritdoc/>
        public void Warning(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
            => new MSBuildMessage(message, id, targetFile, lineStart, columnStart, lineEnd, columnEnd).Warning();

        /// <inheritdoc/>
        public void InnerError(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
            => new MSBuildMessage(message, null, filePath, line).Error();

        /// <inheritdoc/>
        public void Error(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
            => new MSBuildMessage(message, id, targetFile, lineStart, columnStart, lineEnd, columnEnd).Error();

        /// <inheritdoc/>
        public void InnerThrow(string message, [CallerFilePath] string? filePath = null, [CallerLineNumber] int line = 1)
            => throw new MSBuildException(new MSBuildMessage(message, null, filePath, line));

        /// <inheritdoc/>
        public void Throw(string message, string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
            => throw new MSBuildException(new MSBuildMessage(message, id, targetFile, lineStart, columnStart, lineEnd, columnEnd));
    }
}
