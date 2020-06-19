using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 为在 MSBuild 控制台提供用于输出详细警告或错误信息的方法。
    /// </summary>
    internal sealed class MSBuildMessage
    {
        private readonly string _message;
        private string? _id;
        private FileInfo? _file;
        private int? _lineStart;
        private int? _columnStart;
        private int? _lineEnd;
        private int? _columnEnd;

        /// <summary>
        /// 创建用于输出指定 MSBuild 控制台消息的 <see cref="MSBuildMessage"/> 的新实例。
        /// </summary>
        /// <param name="message">编译过程消息，或者警告、错误消息。</param>
        internal MSBuildMessage(string message) => _message = message ?? throw new ArgumentNullException(nameof(message));

        /// <summary>
        /// 创建用于输出指定 MSBuild 控制台消息的 <see cref="MSBuildMessage"/> 的新实例。
        /// </summary>
        /// <param name="message">编译过程消息，或者警告、错误消息。</param>
        /// <param name="id">编译提示/警告/错误的唯一标识符。例如 CS8601，前面使用 2~4 个全大写字母表示编译器代号，后面使用 2~4 个数字表示特定编译警告或错误的编号。</param>
        /// <param name="file">报告的让 IDE 跳转到的文件。</param>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        public MSBuildMessage(string message, string? id = null, string? file = null,
            int? lineStart = null, int? columnStart = null, int? lineEnd = null, int? columnEnd = null)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _id = id;
            _file = new FileInfo(file ?? throw new ArgumentNullException(nameof(file)));
            _lineStart = lineStart;
            _columnStart = columnStart;
            _lineEnd = lineEnd;
            _columnEnd = columnEnd;
        }

        /// <summary>
        /// 为此消息指定编译唯一 Id。
        /// </summary>
        /// <param name="id">编译提示/警告/错误的唯一标识符。例如 CS8601，前面使用 2~4 个全大写字母表示编译器代号，后面使用 2~4 个数字表示特定编译警告或错误的编号。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage Id(string id) => BuildArgument(ref _id, id, nameof(id), "编号");

        /// <summary>
        /// 为此消息指定要让 IDE 跳转到的文件。
        /// </summary>
        /// <param name="file">报告的让 IDE 跳转到的文件。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage File(string file) => BuildArgument(ref _file, new FileInfo(file), nameof(file), "文件");

        /// <summary>
        /// 为此消息指定要让 IDE 跳转到的文件。
        /// </summary>
        /// <param name="file">报告的让 IDE 跳转到的文件。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage File(FileInfo file) => BuildArgument(ref _file, file, nameof(file), "文件");

        /// <summary>
        /// 为此消息指定要让 IDE 跳转到文件的行号。
        /// </summary>
        /// <param name="line">报告的让 IDE 跳转到的文件的行号（序号从 1 开始）。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage TextRange(int line)
            => BuildArgument(ref _lineStart, line, nameof(line), "开始行号")
            .BuildArgument(ref _columnStart, 1, "自动配置", "开始字符号")
            .BuildArgument(ref _lineEnd, line, nameof(line), "结束行号")
            .BuildArgument(ref _columnEnd, 1, "自动配置", "结束字符号");

        /// <summary>
        /// 为此消息指定要让 IDE 跳转到文件的行号和字符号。
        /// </summary>
        /// <param name="line">报告的让 IDE 跳转到的文件的行号（序号从 1 开始）。</param>
        /// <param name="column">报告的让 IDE 跳转到的文件的字符号（序号从 1 开始）。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage TextRange(int line, int column)
            => BuildArgument(ref _lineStart, line, nameof(line), "开始行号")
            .BuildArgument(ref _columnStart, column, nameof(column), "开始字符号")
            .BuildArgument(ref _lineEnd, line, nameof(line), "结束行号")
            .BuildArgument(ref _columnEnd, column, nameof(column), "结束字符号");

        /// <summary>
        /// 为此消息指定要让 IDE 跳转到文件的行和字符范围。
        /// </summary>
        /// <param name="lineStart">报告的让 IDE 跳转到的文件的开始行行号（序号从 1 开始）。</param>
        /// <param name="columnStart">报告的让 IDE 跳转到的文件的开始列列号（序号从 1 开始）。</param>
        /// <param name="lineEnd">报告的让 IDE 跳转到的文件的结束行行号（序号从 1 开始）。</param>
        /// <param name="columnEnd">报告的让 IDE 跳转到的文件的结束列列号（序号从 1 开始）。</param>
        /// <returns>构造器模式。</returns>
        public MSBuildMessage TextRange(int lineStart, int columnStart, int lineEnd, int columnEnd)
            => BuildArgument(ref _lineStart, lineStart, nameof(lineStart), "开始行号")
            .BuildArgument(ref _columnStart, columnStart, nameof(columnStart), "开始字符号")
            .BuildArgument(ref _lineEnd, lineEnd, nameof(lineEnd), "结束行号")
            .BuildArgument(ref _columnEnd, columnEnd, nameof(columnEnd), "结束字符号");

        /// <summary>
        /// 使用构造器模式指定一个参数。
        /// </summary>
        /// <typeparam name="T">编译器自动推断的字段的类型。</typeparam>
        /// <param name="field">字段。</param>
        /// <param name="value">参数值。</param>
        /// <param name="argumentName">参数名。</param>
        /// <param name="argumentDescription">参数描述。</param>
        /// <returns>构造器模式。</returns>
        private MSBuildMessage BuildArgument<T>(ref T field, T value, string argumentName, string argumentDescription)
        {
            if (field != null)
            {
                throw new InvalidOperationException($"已经配置过{argumentDescription}参数 {argumentName}，不能重复配置。");
            }
            field = value ?? throw new ArgumentNullException(argumentName);
            return this;
        }

        /// <summary>
        /// 报告编译消息。
        /// </summary>
        public void Message() => Console.WriteLine(ToString(MessageLevel.Message));

        /// <summary>
        /// 报告编译警告。
        /// </summary>
        public void Warning() => Console.WriteLine(ToString(MessageLevel.Warning));

        /// <summary>
        /// 报告编译错误。
        /// </summary>
        public void Error() => Console.WriteLine(ToString(MessageLevel.Error));

        /// <summary>
        /// 构造一个用于输出到 MSBuild 控制台的消息。
        /// </summary>
        /// <returns>构造好的 MSBuild 控制台消息。可直接使用 Console.WriteLine 输出以产生 MSBuild 编译效果。</returns>
        public string ToString(MessageLevel level)
        {
            var builder = new StringBuilder();
            if (_file != null)
            {
                builder.Append(_file.FullName);
            }
            if (_lineStart != null || _lineEnd != null)
            {
                var lineStartCore = _lineStart ?? _lineEnd;
                var lineEndCore = _lineEnd ?? lineStartCore;
                var columnStartCore = _columnStart ?? 0;
                var columnEndCore = _columnEnd ?? 1;
                builder.Append('(').Append(_lineStart).Append(',').Append(columnStartCore).Append(',').Append(lineEndCore).Append(',').Append(columnEndCore).Append(')');
            }
            if (level == MessageLevel.Message || level == MessageLevel.Error)
            {
                if (builder.Length > 0)
                {
                    builder.Append(": ");
                }
                builder.Append(level.ToString().ToLower(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrWhiteSpace(_id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(' ');
                }
                builder.Append(_id);
            }
            if (builder.Length > 0)
            {
                builder.Append(": ");
            }
            builder.Append(_message);
            return builder.ToString();
        }

        /// <summary>
        /// 构造一个用于输出到 MSBuild 控制台的消息。
        /// </summary>
        /// <returns>构造好的 MSBuild 控制台消息。可直接使用 Console.WriteLine 输出以产生 MSBuild 编译效果。</returns>
        public override string ToString() => ToString(MessageLevel.Message);
    }
}
