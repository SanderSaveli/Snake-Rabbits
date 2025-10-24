using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public class SignalDoPostGameAction
    {
        private struct SubscriberOrder
        {
            public UniTask Action;
            public int Order;

            public SubscriberOrder(UniTask action, int order)
            {
                Action = action;
                Order = order;
            }
        }

        public bool IsWin { get; private set; }
        public IReadOnlyList<UniTask> Subscribers => GetSubscribers();

        private List<SubscriberOrder> _subscribers;

        public SignalDoPostGameAction(bool isWin)
        {
            IsWin = isWin;
            _subscribers = new List<SubscriberOrder>();
        }

        public void AddSubscriber(UniTask action, int order)
        {
            _subscribers.Add(new SubscriberOrder(action, order));
            _subscribers.Sort((a, b) => a.Order - b.Order);
        }

        private IReadOnlyList<UniTask> GetSubscribers()
        {
            List<UniTask> subscribers = new List<UniTask>();
            foreach (var subscriber in _subscribers)
            {
                subscribers.Add(subscriber.Action);
            }
            return subscribers;
        }
    }
}
