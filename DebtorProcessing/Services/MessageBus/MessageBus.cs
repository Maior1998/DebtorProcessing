using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorProcessing.Services.MessageBus
{
    /// <summary>
    /// Представляет собой шину общения разных ViewMaodel между собой.
    /// </summary>
    public class MessageBus
    {
        private readonly ConcurrentDictionary<MessageSubscriber, Func<IMessage, Task>> consumers;

        public MessageBus()
        {
            consumers = new();
        }

        public async Task SendTo<TReceiver>(IMessage message)
        {
            Type messageType = message.GetType();
            Type receiverType = typeof(TReceiver);

            IEnumerable<Task> tasks = consumers
                .Where(s => s.Key.MessageType == messageType && s.Key.ReceiverType == receiverType)
                .Select(s => s.Value(message));

            await Task.WhenAll(tasks);
        }

        public IDisposable Receive<TMessage>(object receiver, Func<TMessage, Task> handler) where TMessage : IMessage
        {
            MessageSubscriber sub = new(receiver.GetType(), typeof(TMessage), s => consumers.TryRemove(s, out Func<IMessage, Task> _));

            consumers.TryAdd(sub, (@event) => handler((TMessage)@event));

            return sub;
        }

        public IDisposable Receive<TMessage>(Type receiverType, Func<TMessage, Task> handler) where TMessage : IMessage
        {
            MessageSubscriber sub = new(receiverType, typeof(TMessage), s => consumers.TryRemove(s, out Func<IMessage, Task> _));

            consumers.TryAdd(sub, (@event) => handler((TMessage)@event));

            return sub;
        }
    }
}
