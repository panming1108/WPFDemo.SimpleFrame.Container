using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.Messager
{
    public class Messager
    {
        private static readonly ConcurrentDictionary<String, object> _cache = new ConcurrentDictionary<String, object>();

        public void Register<TMessage>(object recipient, string token, Func<TMessage, Task> action, bool keepTargetAlive = false)
        {
            var c = action.Target.GetType().Name;
            var m = action.Method.Name;

            Action<TMessage> wrap = async (o) =>
            {
                try
                {
                    await action(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "");
                }
            };

            ///var count = _cache.Count;

            var isSuccess = _cache.TryAdd(
                recipient.GetHashCode() +
                c + m, wrap);

            if (!isSuccess)///count == _cache.Count
            {
                var txt = c + m + "  Register Fail";

                MessageBox.Show(txt, "");
            }

            Messenger.Default.Register<TMessage>(recipient, token, wrap);
        }

        public void Unregister<TMessage>(object recipient, string token, Func<TMessage, Task> action)
        {
            var c = action.Target.GetType().Name;
            var m = action.Method.Name;

            object wrap;

            ///var count = _cache.Count;

            var isSuccess = _cache.TryRemove(
                recipient.GetHashCode() +
                c + m, out wrap);

            if (!isSuccess) ///count == _cache.Count
            {
                var txt = c + m + "  Unregister Fail";
                
                MessageBox.Show(txt, "");
            }

            Messenger.Default.Unregister<TMessage>(recipient, token, (Action<TMessage>)wrap);
        }

        public void Send<TMessage>(string token, TMessage message)
        {
            Messenger.Default.Send<TMessage>(message, token);
        }
    }
}
