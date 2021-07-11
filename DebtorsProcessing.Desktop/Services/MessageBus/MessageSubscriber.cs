
using System;

namespace DebtorsProcessing.Desktop.Services.MessageBus
{
    public class MessageSubscriber : IDisposable
    {
        private readonly Action<MessageSubscriber> action;

        public MessageSubscriber(Type receiverType, Type messageType, Action<MessageSubscriber> action)
        {
            ReceiverType = receiverType;
            MessageType = messageType;
            this.action = action;
        }

        public Type ReceiverType { get; }
        public Type MessageType { get; }

        public void Dispose()
        {
            action?.Invoke(this);
        }
    }
}