using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screenshot
{
    [Designer("Screenshot.ScreenshotDesigner, Screenshot")]
    public class Screenshot : CodeActivity
    {
        [Browsable(false)]
        public string TargetImageBase64 { get; set; }

        public OutArgument<Image> OutImage { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            Image img = ImageConverter.GetImageFromBase64(TargetImageBase64);            
            OutImage.Set(context,img);
        }
    }
}
