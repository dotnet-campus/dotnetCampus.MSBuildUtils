using System;
using System.Globalization;
using System.Text;

namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 表示一个可以完整输出到 MSBuild 控制台的消息。
    /// </summary>
    internal class MSBuildMessage
    {
        /// <summary>
        /// 获取编译过程消息，或者警告、错误消息。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 获取消息提示级别。
        /// </summary>
        private MessageLevel Level { get; }

        /// <summary>
        /// 获取编译提示/警告/错误的唯一标识符。
        /// </summary>
        public string? Id { get; }

        /// <summary>
        /// 获取报告的让 IDE 跳转到的文件。
        /// </summary>
        public string? TargetFile { get; }

        /// <summary>
        /// 获取报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。
        /// </summary>
        private int? LineStart { get; }

        /// <summary>
        /// 获取报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。
        /// </summary>
        private int? ColumnStart { get; }

        /// <summary>
        /// 获取报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。
        /// </summary>
        private int? LineEnd { get; }

        /// <summary>
        /// 获取报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。
        /// </summary>
        private int? ColumnEnd { get; }

        /// <summary>
        /// 创建一个 MSBuild 控制台消息。
        /// </summary>
        /// <param name="message">编译过程消息，或者警告、错误消息。</param>
        /// <param name="level">消息提示级别。</param>
        /// <param name="id">编译提示/警告/错误的唯一标识符。例如 CS8601，前面使用 2~4 个全大写字母表示编译器代号，后面使用 2~4 个数字表示特定编译警告或错误的编号。</param>
        /// <param name="targetFile">报告的让 IDE 跳转到的文件。</param>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        public MSBuildMessage(string message, MessageLevel level = MessageLevel.Message,
            string? id = null, string? targetFile = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Level = level;
            Id = id;
            TargetFile = targetFile ?? throw new ArgumentNullException(nameof(targetFile));
            LineStart = lineStart;
            ColumnStart = columnStart;
            LineEnd = lineEnd;
            ColumnEnd = columnEnd;
        }

        /// <summary>
        /// 构造一个用于输出到 MSBuild 控制台的消息。
        /// </summary>
        /// <returns>构造好的 MSBuild 控制台消息。可直接使用 Console.WriteLine 输出以产生 MSBuild 编译效果。</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(TargetFile))
            {
                builder.Append(TargetFile);
            }
            if (LineStart != null || LineEnd != null)
            {
                var lineStartCore = LineStart ?? LineEnd;
                var lineEndCore = LineEnd ?? lineStartCore;
                var columnStartCore = ColumnStart ?? 0;
                var columnEndCore = ColumnEnd ?? 1;
                builder.Append('(').Append(LineStart).Append(',').Append(columnStartCore).Append(',').Append(lineEndCore).Append(',').Append(columnEndCore).Append(')');
            }
            if (Level == MessageLevel.Message || Level == MessageLevel.Error)
            {
                if (builder.Length > 0)
                {
                    builder.Append(": ");
                }
                builder.Append(Level.ToString().ToLower(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrWhiteSpace(Id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }
                builder.Append(Id);
            }
            if (builder.Length > 0)
            {
                builder.Append(": ");
            }
            builder.Append(Message);
            return builder.ToString();
        }
    }
}
