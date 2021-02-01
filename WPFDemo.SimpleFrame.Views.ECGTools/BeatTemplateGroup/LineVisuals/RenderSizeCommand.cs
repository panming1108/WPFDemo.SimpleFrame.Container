using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public class RenderSizeCommand : ICommand
	{
		private readonly Action<PixelRect> _action;

		public event EventHandler CanExecuteChanged;

		public RenderSizeCommand(Action<PixelRect> action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action((PixelRect)parameter);
		}
	}
}
