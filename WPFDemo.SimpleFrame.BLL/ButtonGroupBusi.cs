﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class ButtonGroupBusi : IButtonGroupBusi
    {
        public Task<List<string>> GetCheckBoxsSource()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var source = new List<string>()
                    {
                        "爸爸",
                        "妈妈",
                        "妹妹",
                        "弟弟",
                        "哥哥",
                    };
                    return source;
                });
        }

        public Task<List<string>> GetRadioButtonsSource()
        {
            return Task.Factory.StartNew(
                ()=> 
                {
                    var source = new List<string>()
                    {
                        "爸爸",
                        "妈妈",
                        "妹妹",
                        "弟弟",
                        "哥哥",
                    };
                    return source;
                });
        }

        public Task<List<Student>> GetRadioButtonsStudentSource()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var source = new List<Student>()
                    {
                        new Student(1,"测试1",11),
                        new Student(2,"测试2",12),
                        new Student(3,"测试3",13),
                        new Student(4,"测试4",14),
                        new Student(5,"测试5",15),
                    };
                    return source;
                });
        }
    }
}
