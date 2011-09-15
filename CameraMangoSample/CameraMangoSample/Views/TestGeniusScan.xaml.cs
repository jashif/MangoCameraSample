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
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using ImageTools;
using System.Diagnostics;
using ImageTools.Controls;
using System.Threading;
using Microsoft.Phone;
using CameraMangoSample.NewEffects;

namespace CameraMangoSample.Views
{
    public partial class TestGeniusScan : PhoneApplicationPage
    {
        public TestGeniusScan()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(TestGeniusScan_Loaded);
        }
        //Fields
        private static ManualResetEvent captureEvent = new ManualResetEvent(true);
        private static ManualResetEvent pauseFramesEvent = new ManualResetEvent(true);
        private WriteableBitmap wb;
        private Thread ImageProductionThread;
        private bool produceFrames;
        PhotoCamera camera;
       

        void TestGeniusScan_Loaded(object sender, RoutedEventArgs e)
        {
            if (null == camera)
            {
                camera = new PhotoCamera();
                camera.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(camera_Initialized);
                camera.CaptureStarted += new EventHandler(camera_CaptureStarted);
                camera.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(camera_CaptureImageAvailable);
                camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camera_CaptureThumbnailAvailable);
                camera.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_CaptureCompleted);
                camBrush.SetSource(camera);
            }
        }

        void camera_CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            // camera.
            //throw new NotImplementedException();
        }

        void camera_CaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        void camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            if (e.ImageStream != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ControlUi.Visibility = System.Windows.Visibility.Collapsed;
                    CameraHolder.Visibility = System.Windows.Visibility.Collapsed;
                    SettingsUI.Visibility = Visibility.Collapsed;
                    PhotoAcceptUi.Visibility = Visibility.Visible;
                    ImageHolderUI.Visibility = System.Windows.Visibility.Visible;
                    ControlUi.Visibility = Visibility.Collapsed;

                    CapturedImage.Stretch = Stretch.Fill;

                    var bmp = PictureDecoder.DecodeJpeg(e.ImageStream, (int)camera.Resolution.Width, (int)camera.Resolution.Height);
                    bmp.Resize((int)this.ActualWidth, (int)this.ActualHeight, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                    if (isBlackEffectSelected)
                    {
                        int[] pixel = temp.Effect.Process(bmp.Pixels,480,640);
                         pixel.CopyTo(bmp.Pixels, 0);
                    }
                    CapturedImage.Source = bmp;

                });
            }
        }

        void camera_CaptureStarted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void camera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                var res = from resolution in camera.AvailableResolutions
                          where resolution.Width == 640
                          select resolution;

                camera.Resolution = res.ToList()[1];
                // camera.Resolution
            }

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Retake_Click(object sender, RoutedEventArgs e)
        {
            ControlUi.Visibility = Visibility.Visible;
            CameraHolder.Visibility = System.Windows.Visibility.Visible;
            SettingsUI.Visibility = Visibility.Collapsed;
            PhotoAcceptUi.Visibility = Visibility.Collapsed;
            ImageHolderUI.Visibility = System.Windows.Visibility.Collapsed;
        }
        //  IEffect effect=
        EffectItem temp = new EffectItem(new BlackWhiteEffect(), "data/icons/BlackWhite.png");



        void ctask_Completed(object sender, Microsoft.Phone.Tasks.PhotoResult e)
        {
            //throw new NotImplementedException();
        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }


        private void HideFlashSettingBtn_Click(object sender, RoutedEventArgs e)
        {
            HidePermissions.Begin();
        }

        private void ShowFlashSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPermissions.Begin();
        }

        private void CaptureImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (camera != null)
            {
                try
                {
                    camera.CaptureImage();
                }
                catch (Exception ex) { }
            }
        }

        private void PaintDropBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPermissions.Stop();
            Storyboard.SetTarget(ShowPermissions, PaintDropUi);
            ShowPermissions.Begin();
            PaintDropUi.Visibility = Visibility.Visible;
        }

        private void ChangeCameraFlashType(object sender, RoutedEventArgs e)
        {
            CameraHolder.Visibility = System.Windows.Visibility.Collapsed;
            MainImage.Visibility = System.Windows.Visibility.Visible;
            StartRendering();

        }

        #region methods
        void ProduceImage()
        {

            int[] ARGBPx = new int[(int)camera.Resolution.Width * (int)camera.Resolution.Height];

            try
            {
                PhotoCamera phCam = camera;

                while (produceFrames)
                {
                    captureEvent.WaitOne();

                    pauseFramesEvent.WaitOne();
                    //Copies the current viewfinder frame into a buffer for further manipulation

                    //Conversion to Seleceted Effect


                    phCam.GetPreviewBufferArgb32(ARGBPx);
                    pauseFramesEvent.Reset();
                    Deployment.Current.Dispatcher.BeginInvoke(delegate()
                    {

                        if (isBlackEffectSelected)
                        {
                            int[] pixel = temp.Effect.Process(wb.Pixels, (int)camera.Resolution.Width, (int)camera.Resolution.Height);
                            pixel.CopyTo(wb.Pixels, 0);
                        }
                        else
                        {
                            ARGBPx.CopyTo(wb.Pixels, 0);

                        }
                       // pixel.CopyTo(wb.Pixels, 0);


                        wb.Invalidate();

                        pauseFramesEvent.Set();
                    });
                }

            }

            catch (Exception e)
            {


            }
        }
        private void StartRendering()
        {
            produceFrames = true;
            wb = new WriteableBitmap((int)camera.Resolution.Width, (int)camera.Resolution.Height);
            ImageProductionThread = new System.Threading.Thread(ProduceImage);
            this.MainImage.Source = wb;
            ImageProductionThread.Start();
        }
        #endregion


        bool isColorEffectSelected = false;
        bool isBlackEffectSelected = false;
        private void colorOrBlack_Click(object sender, RoutedEventArgs e)
        {
           // I//mageProductionThread.Abort();
            var btn = sender as Button;
            if (btn != null)
            {
                if(btn.Content.ToString().Equals(color.Content.ToString()))
                {
                    produceFrames = false;
                    ControlUi.Visibility = Visibility.Visible;
                   //  CameraHolder.Visibility = System.Windows.Visibility.Collapsed;
                    SettingsUI.Visibility = Visibility.Collapsed;
                    PhotoAcceptUi.Visibility = Visibility.Collapsed;
                    ImageHolderUI.Visibility = System.Windows.Visibility.Collapsed;
                    MainImage.Visibility = System.Windows.Visibility.Collapsed; PaintDropUi.Visibility = System.Windows.Visibility.Collapsed;
                    isColorEffectSelected=true;
                }
                if(btn.Content.ToString().Equals(black.Content.ToString()))
                {
                    isBlackEffectSelected = true; ControlUi.Visibility = Visibility.Visible;
                    //   CameraHolder.Visibility = System.Windows.Visibility.Collapsed;
                    SettingsUI.Visibility = Visibility.Collapsed;
                    PhotoAcceptUi.Visibility = Visibility.Collapsed;
                    ImageHolderUI.Visibility = System.Windows.Visibility.Collapsed;
                    MainImage.Visibility = System.Windows.Visibility.Visible; PaintDropUi.Visibility = System.Windows.Visibility.Collapsed;
                    StartRendering();
                }
                
            }

        }
    }
}