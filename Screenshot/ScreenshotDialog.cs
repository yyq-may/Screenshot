using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screenshot
{
    
    public class ScreenshotDialog : Form
    {
        public delegate void TransfDelegate(Bitmap bmp);

        public event TransfDelegate TransfEvent;

        private bool _catchStart;

        private Point _downPoint;

        private Bitmap _bitmap;

        private Rectangle _catchRec;

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);
        [DllImport("user32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);
        public ScreenshotDialog()
        {
            this.AllowTransparency = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Load += ScreenshotDialog_Load;
            this.ShowInTaskbar = false;            
            this._bitmap = GetBitmap();
            this.BackgroundImage = _bitmap;
            this.DoubleBuffered = true;
            this.KeyDown += ScreenshotDialog_KeyDown;
            this.MouseMove += Screenshot_MouseMove;
            this.MouseDown += Screenshot_MouseDown;
            this.MouseUp += Screenshot_MouseUp;
        }

        private void ScreenshotDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void ScreenshotDialog_Load(object sender, EventArgs e)
        {
            Bitmap a = Properties.Resources.pointer;
            SetCursor(a, new Point(0, 0));
        }

        public void SetCursor(Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width, cursor.Height);
            this.Cursor = new Cursor(myNewCursor.GetHicon());
            g.Dispose();
            myNewCursor.Dispose();
        }



        public Bitmap GetBitmap()
        {
            Rectangle rc = SystemInformation.VirtualScreen;
            var bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
            {
                memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }

        private void Screenshot_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_catchStart)
                {
                    _catchStart = false;
                    if (_catchRec.Width > 0 && _catchRec.Height > 0)
                    {
                        Bitmap catchedBmp = new Bitmap(_catchRec.Width, _catchRec.Height);
                        Graphics g = Graphics.FromImage(catchedBmp);
                        g.DrawImage(_bitmap, new Rectangle(0, 0, _catchRec.Width, _catchRec.Height), _catchRec, GraphicsUnit.Pixel);
                        TransfEvent(catchedBmp);
                    }                    
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void Screenshot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_catchStart)
                {
                    _catchStart = true;
                    _downPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                }
            }else if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Screenshot_MouseMove(object sender, MouseEventArgs e)
        {
            if (_catchStart)
            {
                this.Refresh();
                Pen p = new Pen(Color.Red, 1.5f);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                Graphics g = this.CreateGraphics();
                _catchRec = GetRect(_downPoint, e.Location);
                if (_catchRec == Rectangle.Empty)
                {
                    return;
                }
                g.DrawRectangle(p, _catchRec);
                p.Dispose();
                g.Dispose();
            }
        }
        private Rectangle GetRect(Point start, Point end)
        {
            Rectangle rect = new Rectangle();
            rect.Width = Math.Abs(start.X - end.X);
            rect.Height = Math.Abs(start.Y - end.Y);
            if (rect.Width == 0 || rect.Height == 0)
            {
                return Rectangle.Empty;
            }
            if (start.X > end.X && start.Y < end.Y)
            {
                rect.Location = new Point(end.X, start.Y);
            }
            else if (start.X < end.X && start.Y > end.Y)
            {
                rect.Location = new Point(start.X, end.Y);
            }
            else if (start.X > end.X && start.Y > end.Y)
            {
                rect.Location = end;
            }
            else
            {
                rect.Location = start;
            }
            return rect;
        }
    }
}