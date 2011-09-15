using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace CameraMangoSample.Controller
{
    public class ImageInstance
    {
        private static ImageInstance instance;
        public static ImageInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImageInstance();
                }
                return instance;
            }
        }

        public BitmapImage SelectedImage
        {
            get;
            set;
        }
    }
}
