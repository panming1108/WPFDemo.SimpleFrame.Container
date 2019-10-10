using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Messager
{
    public class MessagerInstance
    {
        private static Messager _messager;

        private MessagerInstance()
        {

        }

        public static void Init(Messager messager)
        {
            _messager = messager;
        }

        public static Messager GetMessager()
        {
            return _messager;
        }
    }
}
