using Comet.Common.Networking;
using System;
using System.Threading;

namespace Comet.Common.Messages
{
    /// <summary>
    /// Provides a MessageProcessor implementation that provides progress report callbacks.
    /// </summary>
    /// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
    /// <remarks>
    /// Any event handlers registered with the <see cref="ProgressChanged"/> event are invoked through a 
    /// <see cref="System.Threading.SynchronizationContext"/> instance chosen when the instance is constructed.
    /// </remarks>
    public abstract class MessageProcessorBase<T> : IMessageProcessor, IProgress<T>
    {
        /// <summary>
        /// 在构造时选择的同步上下文。(在各种同步模型中提供传播同步上下文的基本功能 )
        /// </summary>
        protected readonly SynchronizationContext SynchronizationContext;

        /// <summary>
        /// 创建委托，方法是将回调方法传递给SendOrPostCallback 构造函数。您的方法必须具有此处所显示的签名。
        /// </summary>
        private readonly SendOrPostCallback _invokeReportProgressHandlers;

        /// <summary>
        /// Represents the method that will handle progress updates.
        /// </summary>
        /// <param name="sender">The message processor which updated the progress.</param>
        /// <param name="value">The new progress.</param>
        public delegate void ReportProgressEventHandler(object sender, T value);

        /// <summary>
        /// Raised for each reported progress value.
        /// </summary>
        /// <remarks>
        /// Handlers registered with this event will be invoked on the 
        /// <see cref="System.Threading.SynchronizationContext"/> chosen when the instance was constructed.
        /// </remarks>
        public event ReportProgressEventHandler ProgressChanged;

        /// <summary>
        /// 进度更改报告
        /// </summary>
        /// <param name="value">The value of the updated progress.</param>
        protected virtual void OnReport(T value)
        {
            //如果没有处理程序，就不需要遍历同步上下文。
            //在回调函数中，我们需要再次检查，以防万一删除事件处理程序。
            ReportProgressEventHandler handler = ProgressChanged;
            if (handler != null)
            {
                //在线程池上去调用委托来实现（异步调用）
                SynchronizationContext.Post(_invokeReportProgressHandlers, value);
            }
        }

        /// <summary>
        /// Initializes the <see cref="MessageProcessorBase{T}"/>
        /// </summary>
        /// <param name="useCurrentContext">
        /// If this value is <c>false</c>, the progress callbacks will be invoked on the ThreadPool.
        /// Otherwise the current SynchronizationContext will be used.
        /// </param>
        protected MessageProcessorBase(bool useCurrentContext)
        {
            _invokeReportProgressHandlers = InvokeReportProgressHandlers;
            SynchronizationContext = useCurrentContext ? SynchronizationContext.Current : ProgressStatics.DefaultContext;
        }

        /// <summary>
        /// 调用进度事件回调。
        /// </summary>
        /// <param name="state">The progress value.</param>
        private void InvokeReportProgressHandlers(object state)
        {
            var handler = ProgressChanged;
            handler?.Invoke(this, (T)state);
        }

        /// <inheritdoc />
        public abstract bool CanExecute(IMessage message);

        /// <inheritdoc />
        public abstract bool CanExecuteFrom(ISender sender);

        /// <inheritdoc />
        public abstract void Execute(ISender sender, IMessage message);

        void IProgress<T>.Report(T value) => OnReport(value);
    }

    /// <summary>
    /// Holds static values for <see cref="MessageProcessorBase{T}"/>.
    /// </summary>
    /// <remarks>
    /// This avoids one static instance per type T.
    /// </remarks>
    internal static class ProgressStatics
    {
        /// <summary>
        /// A default synchronization context that targets the <see cref="ThreadPool"/>.
        /// </summary>
        internal static readonly SynchronizationContext DefaultContext = new SynchronizationContext();
    }
}
