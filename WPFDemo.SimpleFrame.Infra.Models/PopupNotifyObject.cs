using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Enums;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class PopupNotifyObject
    {
        public PopupNotifyObject()
        {

        }
        public PopupNotifyObject(string title, string info, PopupNotifyEnum popupNotifyEnum = PopupNotifyEnum.Message)
        {
            PopupNotifyEnum = popupNotifyEnum;
            Title = title;
            Info = info;
        }

        public PopupNotifyEnum PopupNotifyEnum { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }
    }
}
