﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WPFDemo.SimpleFrame.Infra.Ioc;

namespace WPFDemo.SimpleFrame.Container
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var mutex = new Mutex(true, "WPFDemo.SimpleFrame.Container", out bool createNew);
            if(createNew)
            {
                string[] list = new string[]
                {
                    "WPFDemo.SimpleFrame.BLL",
                    "WPFDemo.SimpleFrame.DAL",

                    "WPFDemo.SimpleFrame.ViewModels.Test",
                    "WPFDemo.SimpleFrame.ViewModels.DMs",
                    "WPFDemo.SimpleFrame.ViewModels.DVs",
                    "WPFDemo.SimpleFrame.ViewModels.LayOut",
                    "WPFDemo.SimpleFrame.ViewModels.UXs",
                    "WPFDemo.SimpleFrame.ViewModels.Navis",
                    "WPFDemo.SimpleFrame.ViewModels.Editors",
                    "WPFDemo.SimpleFrame.ViewModels.ECGTools",

                    "WPFDemo.SimpleFrame.Views.LayOut",
                    "WPFDemo.SimpleFrame.Views.CustomDialogs",

                    "WPFDemo.SimpleFrame.Infra.CustomControls",
                    "WPFDemo.SimpleFrame.Infra.DialogConsumer",

                    "WPFDemo.SimpleFrame.Container",
                };

                IocManager.RegisterAssemblyInterfaces(list);

                IocManagerInstance.Init(new IocManager());

                Startup startup = new Startup();
                startup.Run();
                mutex.WaitOne();
            }
        }
    }
}
