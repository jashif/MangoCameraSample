using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace CameraMangoSample.Views
{
    public partial class ImageView : PhoneApplicationPage
    {
        public ImageView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ImageView_Loaded);
        }

        void ImageView_Loaded(object sender, RoutedEventArgs e)
        {
            image1.Source = Controller.ImageInstance.Instance.SelectedImage;
        }
    }
}