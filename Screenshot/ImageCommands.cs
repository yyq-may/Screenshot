using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Screenshot
{
    public class ImageCommands
    {
		public static readonly ICommand Grab = new RoutedCommand();


		public static readonly ICommand Load = new RoutedCommand();


		public static readonly ICommand Save = new RoutedCommand();


		public static readonly ICommand IndicateScope = new RoutedCommand();


		public static readonly ICommand AddToLibrary = new RoutedCommand();
	}
}
