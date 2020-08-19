using System;
using System.Runtime.Serialization;

namespace dotnetCampus.MSBuildUtils
{
    /// <summary>
    /// 抛出此异常以表示这是编译过程中目标项目的错误（而不是编译器或者编译扩展程序的内部错误）。
    /// </summary>
    [Serializable]
    internal class MSBuildException : Exception
    {
        private static readonly string DefaultMessage = "项目在编译过程中发生了错误，请修正错误后重新编译。";

        /// <summary>
        /// 获取 MSBuild 控制台消息。
        /// </summary>
        internal MSBuildMessage MSBuildMessage { get; }

        /// <summary>
        /// 尽量不要使用此构造函数来创建，因为这样会缺少有效的信息用于修复编译错误。
        /// </summary>
        public MSBuildException() : base(DefaultMessage)
        {
            MSBuildMessage = new MSBuildMessage(DefaultMessage);
        }

        /// <inheritdoc />
        public MSBuildException(string message) : base(message)
        {
            MSBuildMessage = new MSBuildMessage(message);
        }

        /// <inheritdoc />
        public MSBuildException(string message, Exception innerException) : base(message, innerException)
        {
            MSBuildMessage = new MSBuildMessage(message);
        }

        /// <summary>
        /// 用一个 MSBuild 控制台消息创建一个编译错误。
        /// </summary>
        /// <param name="message">MSBuild 控制台消息。</param>
        internal MSBuildException(MSBuildMessage message)
            : base(message?.ToString(MessageLevel.Error) ?? throw new ArgumentNullException(nameof(message)))
        {
            MSBuildMessage = message;
        }

        /// <summary>
        /// 用一个 MSBuild 控制台消息创建一个包含内部异常的编译错误。
        /// </summary>
        /// <param name="message">MSBuild 控制台消息。</param>
        /// <param name="innerException">内部异常。</param>
        internal MSBuildException(MSBuildMessage message, Exception innerException)
            : base(message?.ToString(MessageLevel.Error) ?? throw new ArgumentNullException(nameof(message)), innerException)
        {
            MSBuildMessage = message;
        }

        /// <inheritdoc />
        protected MSBuildException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            MSBuildMessage = new MSBuildMessage(DefaultMessage);
        }

        /// <summary>
        /// 将异常作为编译错误输出到 MSBuild 控制台。
        /// </summary>
        public void ReportBuildError()
        {
            MSBuildMessage.Error();
        }
    }
}
