using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class TreeViewNode
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string InputGestureText { get; set; }

        public List<TreeViewNode> Children { get; set; }

        public TreeViewNode()
        {

        }

        public TreeViewNode(string name)
        {
            Name = name;
        }

        public TreeViewNode(string icon, string name) : this(name)
        {
            Icon = icon;
        }

        public TreeViewNode(string icon, string name, string inputGestureText) : this(icon, name)
        {
            InputGestureText = inputGestureText;
        }

        public TreeViewNode(string icon, string name, string inputGestureText, List<TreeViewNode> children) : this(icon, name, inputGestureText)
        {
            Children = children;
        }
    }
}
