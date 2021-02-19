using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatDetailViewContextMenuHelpler : IDisposable
    {
        private readonly NavigationContextMenuSource _navigationContextMenuSource;
        private readonly UpdateBeatContextMenuSource _updateBeatContextMenuSource;
        private readonly OperateContextMenuSource _operateContextMenuSource;
        private readonly AFContextMenuSource _aFContextMenuSource;

        public BeatDetailViewContextMenuHelpler(RoutedEventHandler updateRoutedEventHandler, 
            RoutedEventHandler deleteHandler, RoutedEventHandler insertBeatToPrevHandler, RoutedEventHandler insertBeatToNextHandler, RoutedEventHandler unConfuseHandler, RoutedEventHandler setLongestRRHandler,
            RoutedEventHandler signAFRoutedEventHandler)
        {
            _navigationContextMenuSource = new NavigationContextMenuSource();
            _aFContextMenuSource = new AFContextMenuSource(signAFRoutedEventHandler, null);
            _updateBeatContextMenuSource = new UpdateBeatContextMenuSource(updateRoutedEventHandler);
            _operateContextMenuSource = new OperateContextMenuSource(deleteHandler, insertBeatToPrevHandler, insertBeatToNextHandler, unConfuseHandler, setLongestRRHandler, null);
        }

        public List<Control> GetSingleSelectMenuItems()
        {
            List<Control> singleSelectMenuItems = new List<Control>();
            singleSelectMenuItems.AddRange(_navigationContextMenuSource.GetAllMenuItems());
            singleSelectMenuItems.Add(new Separator());
            singleSelectMenuItems.Add(_aFContextMenuSource.SignAf);
            singleSelectMenuItems.Add(_aFContextMenuSource.SignAF);
            singleSelectMenuItems.Add(_aFContextMenuSource.DeleteAllAf);
            singleSelectMenuItems.Add(_aFContextMenuSource.DeleteAllAF);
            singleSelectMenuItems.Add(new Separator());
            singleSelectMenuItems.AddRange(_updateBeatContextMenuSource.GetAllMenuItems());
            singleSelectMenuItems.Add(new Separator());
            singleSelectMenuItems.Add(_operateContextMenuSource.DeleteBeat);
            singleSelectMenuItems.Add(_operateContextMenuSource.InsertBeatToPrev);
            singleSelectMenuItems.Add(_operateContextMenuSource.InsertBeatToNext);
            singleSelectMenuItems.Add(_operateContextMenuSource.UnConfuse);
            singleSelectMenuItems.Add(_operateContextMenuSource.PWaveUnConfuse);
            singleSelectMenuItems.Add(_operateContextMenuSource.SetLongestRR);
            return singleSelectMenuItems;
        }

        public List<Control> GetBatchSelectMenuItems()
        {
            List<Control> batchSelectMenuItems = new List<Control>();
            batchSelectMenuItems.Add(_aFContextMenuSource.SignAf);
            batchSelectMenuItems.Add(_aFContextMenuSource.SignAF);
            batchSelectMenuItems.Add(_aFContextMenuSource.DeleteAllAf);
            batchSelectMenuItems.Add(_aFContextMenuSource.DeleteAllAF);
            batchSelectMenuItems.Add(new Separator());
            batchSelectMenuItems.AddRange(_updateBeatContextMenuSource.GetAllMenuItems());
            batchSelectMenuItems.Add(new Separator());
            batchSelectMenuItems.Add(_operateContextMenuSource.DeleteBeat);
            batchSelectMenuItems.Add(_operateContextMenuSource.InsertBeatToPrev);
            batchSelectMenuItems.Add(_operateContextMenuSource.InsertBeatToNext);
            batchSelectMenuItems.Add(_operateContextMenuSource.UnConfuse);
            batchSelectMenuItems.Add(_operateContextMenuSource.PWaveUnConfuse);
            batchSelectMenuItems.Add(_operateContextMenuSource.SetLongestRR);
            return batchSelectMenuItems;
        }

        public void Dispose()
        {
            _navigationContextMenuSource.Dispose();
            _updateBeatContextMenuSource.Dispose();
            _operateContextMenuSource.Dispose();
            _aFContextMenuSource.Dispose();
        }
    }
}
