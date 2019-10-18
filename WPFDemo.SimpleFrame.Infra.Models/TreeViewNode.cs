using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class TreeViewNode
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public List<TreeViewNode> Children { get; set; }

        public TreeViewNode()
        {

        }

        public TreeViewNode(string name)
        {
            Name = name;
        }

        public TreeViewNode(string icon, string name)
        {
            Icon = icon;
            Name = name;
        }
    }
}
