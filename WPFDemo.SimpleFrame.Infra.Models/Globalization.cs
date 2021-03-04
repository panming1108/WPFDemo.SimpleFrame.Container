using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public static class Globalization
    {
        private static ResourceManager _resourceManager;
        public static ResourceManager ResourceManager => _resourceManager;
        public static void Init(LangueageEnum langueage)
        {
            switch (langueage)
            {
                case LangueageEnum.Chinese:
                    _resourceManager = Properties.Chinese.ResourceManager;
                    break;
                case LangueageEnum.English:
                    _resourceManager = Properties.English.ResourceManager;
                    break;
                default:
                    _resourceManager = Properties.Chinese.ResourceManager;
                    break;
            }
        }

        public static string TestString => _resourceManager.GetString(nameof(TestString));
    }

    public enum LangueageEnum
    {
        Chinese,
        English
    }
}
