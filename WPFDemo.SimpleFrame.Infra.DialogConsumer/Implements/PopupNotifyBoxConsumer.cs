using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFDemo.SimpleFrame.Infra.DialogConsumer.Interfaces;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.IViews.CustomDialogs;

namespace WPFDemo.SimpleFrame.Infra.DialogConsumer.Implements
{
    public class PopupNotifyBoxConsumer : IPopupNotifyBoxConsumer
    {
        private Window _window;

        public PopupNotifyBoxConsumer()
        {
            Messager.MessagerInstance.GetMessager().Register<PopupNotifyObject>(this, MessagerKeyEnum.PopupNotifyBox, OnPopupNotify);
        }

        private async Task OnPopupNotify(PopupNotifyObject popupNotifyObject)
        {
            await TaskEx.FromResult(0);
            DispatcherHelper.InvokeOnUIThread(
                () =>
                {
                    switch (popupNotifyObject.PopupNotifyEnum)
                    {
                        case PopupNotifyEnum.Message:
                            var messageView = IocManagerInstance.ResolveType<IPopupMessageView>();
                            messageView.PlacementTarget = _window;
                            messageView.Show(popupNotifyObject.Title, popupNotifyObject.Info);
                            break;
                        default:
                            break;
                    }
                });
        }

        public void Dispose()
        {
            Messager.MessagerInstance.GetMessager().Unregister<PopupNotifyObject>(this, MessagerKeyEnum.PopupNotifyBox, OnPopupNotify);
        }

        public void Init(Window window)
        {
            _window = window;
        }
    }
}
