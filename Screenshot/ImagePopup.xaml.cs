using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Screenshot
{
    /// <summary>
    /// ImagePopup.xaml 的交互逻辑
    /// </summary>
    public partial class ImagePopup : Window, INotifyPropertyChanged
    {
        public ImagePopup()
        {
            InitializeComponent();
            
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        private string _informativeImageBase64;
        public string InformativeScreenshotBase64
        {
            get
            {
                return this._informativeImageBase64;
            }
            set
            {
                this._informativeImageBase64 = value;
                this.OnNotifyPropertyChanged("InformativeScreenshotBase64");
            }
        }
        private void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
