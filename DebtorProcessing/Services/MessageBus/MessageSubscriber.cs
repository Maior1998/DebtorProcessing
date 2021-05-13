using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorProcessing.Services.MessageBus
{
    public class MessageSubscriber : IDisposable
    {
        private readonly Action<MessageSubscriber> action;

        public Type ReceiverType { get; }
        public Type MessageType { get; }

        public MessageSubscriber(Type receiverType, Type messageType, Action<MessageSubscriber> action)
        {
            ReceiverType = receiverType;
            MessageType = messageType;
            this.action = action;
        }

        public void Dispose()
        {
            action?.Invoke(this);
        }
    }
}
