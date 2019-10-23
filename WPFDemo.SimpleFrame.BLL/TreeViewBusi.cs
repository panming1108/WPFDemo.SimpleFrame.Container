using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class TreeViewBusi : ITreeViewBusi
    {
        public Task<List<TreeViewNode>> GetMenuSource()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    List<TreeViewNode> nodes = new List<TreeViewNode>();
                    var node11 = new TreeViewNode("\uf000", "菜单");
                    nodes.Add(node11);

                    node11.Children = new List<TreeViewNode>();
                    node11.Children.Add(new TreeViewNode("\uf001", "跳转到全图"));
                    node11.Children.Add(new TreeViewNode("\uf002", "跳转到诊断图"));
                    node11.Children.Add(new TreeViewNode("\uf003", "删除心搏"));
                    node11.Children.Add(new TreeViewNode("\uf004", "室上性"));
                    node11.Children.Add(new TreeViewNode("\uf005", "正常(N)"));
                    node11.Children.Add(new TreeViewNode("\uf006", "房早(S)"));

                        var qibo = new TreeViewNode("\uf007", "起搏(P)");
                        qibo.Children = new List<TreeViewNode>();
                        qibo.Children.Add(new TreeViewNode("\uf008", "室上性"));
                        qibo.Children.Add(new TreeViewNode("\uf009", "正常"));
                        qibo.Children.Add(new TreeViewNode("\uf010", "房早"));
                        qibo.Children.Add(new TreeViewNode("\uf011", "室上性"));
                        qibo.Children.Add(new TreeViewNode("\uf012", "室上性"));

                            var yujibo = new TreeViewNode("\uf013", "预激波");
                            yujibo.Children = new List<TreeViewNode>();
                            yujibo.Children.Add(new TreeViewNode("\uf014", "窦性(N(B))"));
                            yujibo.Children.Add(new TreeViewNode("\uf015", "房性(N(B))"));

                        qibo.Children.Add(yujibo);

                    node11.Children.Add(qibo);

                    return nodes;
                });
        }

        public Task<List<TreeViewNode>> GetTreeViewIconSource()
        {
            return Task.Factory.StartNew(
                () => 
                {
                    List<TreeViewNode> nodes = new List<TreeViewNode>();
                    var node11 = new TreeViewNode("\uf000", "版本介绍");
                    var node12 = new TreeViewNode("\uf001", "UI");
                    var node13 = new TreeViewNode("\uf002", "系统设置");
                    var node14 = new TreeViewNode("\uf003", "高级设置");
                    nodes.Add(node11);
                    nodes.Add(node12);
                    nodes.Add(node13);
                    nodes.Add(node14);

                    node12.Children = new List<TreeViewNode>();
                    node12.Children.Add(new TreeViewNode("\uf004", "输入"));
                    node12.Children.Add(new TreeViewNode("\uf005", "按钮"));
                    node12.Children.Add(new TreeViewNode("\uf006", "列表"));
                    node12.Children.Add(new TreeViewNode("\uf007", "波形回顾"));
                    node12.Children.Add(new TreeViewNode("\uf008", "实时波形"));
                   
                    node13.Children = new List<TreeViewNode>();
                    node13.Children.Add(new TreeViewNode("\uf009", "基础设置"));
                    node13.Children.Add(new TreeViewNode("\uf010", "设备设置"));

                    return nodes;
                });
        }

        public Task<List<TreeViewNode>> GetTreeViewImageSource()
        {
            string imgaeSource = "/WPFDemo.SimpleFrame.Views.LayOut;component/Images/urgent_1.png";
            return Task.Factory.StartNew(
                () =>
                {
                    List<TreeViewNode> nodes = new List<TreeViewNode>();
                    var node11 = new TreeViewNode(imgaeSource, "版本介绍");
                    var node12 = new TreeViewNode(imgaeSource, "UI");
                    var node13 = new TreeViewNode(imgaeSource, "系统设置");
                    var node14 = new TreeViewNode(imgaeSource, "高级设置");
                    nodes.Add(node11);
                    nodes.Add(node12);
                    nodes.Add(node13);
                    nodes.Add(node14);

                    node12.Children = new List<TreeViewNode>();
                    node12.Children.Add(new TreeViewNode(imgaeSource, "输入"));
                    node12.Children.Add(new TreeViewNode(imgaeSource, "按钮"));
                    node12.Children.Add(new TreeViewNode(imgaeSource, "列表"));
                    node12.Children.Add(new TreeViewNode(imgaeSource, "波形回顾"));
                    node12.Children.Add(new TreeViewNode(imgaeSource, "实时波形"));

                    node13.Children = new List<TreeViewNode>();
                    node13.Children.Add(new TreeViewNode(imgaeSource, "基础设置"));
                    node13.Children.Add(new TreeViewNode(imgaeSource, "设备设置"));

                    return nodes;
                });
        }
    }
}
