using Comet.Common.Networking;

namespace Comet.Common.Messages
{
    /// <summary>
    /// Provides basic functionality to process messages.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// 判断这个消息处理器是否可以执行指定的消息.
        /// </summary>
        /// <param name="message">The message to execute.</param>
        /// <returns><c>True</c> if the message can be executed by this message processor, otherwise <c>false</c>.</returns>
        bool CanExecute(IMessage message);

        /// <summary>
        /// 从发送方接收的消息决定此消息处理器是否可以执行
        /// </summary>
        /// <param name="sender">The sender of a message.</param>
        /// <returns><c>True</c> 如果此消息处理程序可以执行来自发送方的消息, 否则 <c>false</c>.</returns>
        bool CanExecuteFrom(ISender sender);

        /// <summary>
        /// 执行收到的消息。
        /// </summary>
        /// <param name="sender">The sender of this message.</param>
        /// <param name="message">The received message.</param>
        void Execute(ISender sender, IMessage message);
    }
}
