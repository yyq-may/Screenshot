using OP.Native.Utils;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static OP.Native.Import.NativeMethods;

namespace Screenshot
{
    /// <summary>
    /// ScreenshotDesigner.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotDesigner
    {
        public ScreenshotDesigner()
        {
            InitializeComponent();
            this.OptionsMenuButton.ContextMenu.PlacementTarget = this.OptionsMenuButton;            
        }
              
        private void OpenOptions_Click(object sender, RoutedEventArgs e)
        {
            this.OptionsMenuButton.ContextMenu.IsOpen = true;
        }
        private void Dialog_TransfEvent(Bitmap bmp)
        {
            base.ModelItem.Properties["TargetImageBase64"].SetValue(ImageConverter.GetBase64FromImage(bmp));
        }
        public void ShowImage(object sender, EventArgs e)
        {
            string text = this.ModelItem.Properties["TargetImageBase64"].ComputedValue.ToString();
            var imgPopup = new ImagePopup();
            imgPopup.InformativeScreenshotBase64 = text;
            imgPopup.ShowDialog();
        }
        private void GrabImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            ShowWindow(process.MainWindowHandle, (int)ShowState.SW_SHOWMINIMIZED);
            var windowStyle = WindowUtils.GetFormState(process.MainWindowHandle);
            if (windowStyle == FormWindowState.Minimized)
            {
                Thread.Sleep(800);
                ScreenshotDialog dialog = new ScreenshotDialog();
                dialog.TransfEvent += Dialog_TransfEvent;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ShowWindow(process.MainWindowHandle, (int)ShowState.SW_RESTORE);
                }
            }
        }

        private void LoadImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "ImageControlDialogTitle";
            openFileDialog.Filter = "ImageFilter" + "|*.png;*.jpg;*.jpeg;*.bmp|PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp";
            bool? flag = openFileDialog.ShowDialog();
            bool flag2 = true;
            if (!(flag.GetValueOrDefault() == flag2 & flag != null) || string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }
            try
            {
                Bitmap image = new Bitmap(openFileDialog.FileName);
                base.ModelItem.Properties["TargetImageBase64"].SetValue(ImageConverter.GetBase64FromImage(image));
            }
            catch
            {
                throw new Exception("Could Not Load Image Exception");
            }
        }

        private void SaveImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Title = "MenuItemSaveImage";
            saveFileDialog.Filter = "ImageFilter" + "|*.png;*.jpg;*.jpeg;*.bmp|PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp";
            saveFileDialog.FileName = "Untitled" + ".png";
            bool? flag = saveFileDialog.ShowDialog();
            bool flag2 = true;
            if ((flag.GetValueOrDefault() == flag2 & flag != null) && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                try
                {
                    ImageConverter.GetImageFromBase64(base.ModelItem.Properties["TargetImageBase64"].Value.ToString()).Save(saveFileDialog.FileName);                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
