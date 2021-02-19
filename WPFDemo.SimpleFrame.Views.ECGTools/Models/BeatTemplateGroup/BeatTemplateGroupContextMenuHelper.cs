using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class BeatTemplateGroupContextMenuHelper : IDisposable
    {
        private readonly UpdateBeatContextMenuSource _updateBeatContextMenuSource;
        private readonly OperateContextMenuSource _operateContextMenuSource;
        private readonly AFContextMenuSource _aFContextMenuSource;

        public BeatTemplateGroupContextMenuHelper(RoutedEventHandler updateRoutedEventHandler,
            RoutedEventHandler deleteHandler, RoutedEventHandler mergeBeatHandler, RoutedEventHandler unConfuseHandler,
            RoutedEventHandler signAFRoutedEventHandler, RoutedEventHandler _cancelSignAfOrAFRoutedEventHandler)
        {
            _aFContextMenuSource = new AFContextMenuSource(signAFRoutedEventHandler, _cancelSignAfOrAFRoutedEventHandler);
            _updateBeatContextMenuSource = new UpdateBeatContextMenuSource(updateRoutedEventHandler);
            _operateContextMenuSource = new OperateContextMenuSource(deleteHandler, null, null, unConfuseHandler, null, mergeBeatHandler);
        }

        public List<Control> GetMenuItems()
        {
            List<Control> menuItems = new List<Control>();
            menuItems.Add(_aFContextMenuSource.SignAf);
            menuItems.Add(_aFContextMenuSource.SignAF);
            menuItems.Add(_aFContextMenuSource.CancelSignAfOrAF);
            menuItems.Add(new Separator());
            menuItems.AddRange(_updateBeatContextMenuSource.GetAllMenuItems());
            menuItems.Add(new Separator());
            menuItems.Add(_operateContextMenuSource.DeleteBeat);
            menuItems.Add(_operateContextMenuSource.MergeBeat);
            menuItems.Add(_operateContextMenuSource.UnConfuse);
            menuItems.Add(_operateContextMenuSource.PWaveUnConfuse);
            return menuItems;
        }

        public void Dispose()
        {
            _updateBeatContextMenuSource.Dispose();
            _operateContextMenuSource.Dispose();
            _aFContextMenuSource.Dispose();
        }
    }
}
