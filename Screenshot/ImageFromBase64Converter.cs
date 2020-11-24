using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Screenshot
{
    public class ImageFromBase64Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value != null)
			{
				try
				{
					string text = value as string;
					if (text != null)
					{
						return new PngBitmapDecoder(new MemoryStream(System.Convert.FromBase64String(text)), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
			return null;
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
			return null;
		}
    }
}
