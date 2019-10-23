using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IButtonGroupBusi
    {
        Task<List<string>> GetRadioButtonsSource();
        Task<List<Student>> GetRadioButtonsStudentSource();
        Task<List<string>> GetCheckBoxsSource();
    }
}
