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
using PhotoFun.Effects;
using System.Windows.Resources;
using System.Threading;
using CameraMangoSample.ViewModel;
using CameraMangoSample.Controller;

namespace CameraMangoSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        //Fields
        PhotoCamera camera; 
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        // Holds total focus time
        public long timeForAutoFocus;

        private static ManualResetEvent captureEvent = new ManualResetEvent(true);
        private static ManualResetEvent pauseFramesEvent = new ManualResetEvent(true);
        private WriteableBitmap wb;
        private Thread ImageProductionThread;
        private bool prouduceFrames;
        private EffectDetails myVar = new EffectDetails() { Effect = new EchoEffect(), IsSelected = true, Name = "EchoEffect" };
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            bordercap.Visibility = System.Windows.Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            var vm = this.DataContext as MainViewModel;
         
            if (null == camera)
            {
                camera = new PhotoCamera();

                camera.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(camera_Initialized);

                //Event is fired when the button is half pressed
                // camera.ButtonHalfPress += camera_ButtonHalfPress;

                //Event is fired when the button is fully pressed
                //   camera.ButtonFullPress += camera_ButtonFullPress;

                //Event is fired when the capture sequence is complete and an image is available.
                camera.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(camera_CaptureImageAvailable);
                camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camera_CaptureThumbnailAvailable);
                camera.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_CaptureCompleted);
                camera.AutoFocusCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_AutoFocusCompleted);

                // The event is fired when the shutter button receives a half press.
                CameraButtons.ShutterKeyHalfPressed += new EventHandler(CameraButtons_ShutterKeyHalfPressed);

                // The event is fired when the shutter button receives a full press.
                CameraButtons.ShutterKeyPressed += new EventHandler(CameraButtons_ShutterKeyPressed);

                // The event is fired when the shutter button is released.
                CameraButtons.ShutterKeyReleased += OnButtonRelease;
                //Set the VideoBrush source to the camera

            } 
            CameraBrush.SetSource(camera);
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            camera = null;
            base.OnNavigatedFrom(e);
        }
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if (e.Orientation == PageOrientation.LandscapeRight)
            {
                CameraBrush.RelativeTransform =
                    new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 180 };
            }
            else
            {
                CameraBrush.RelativeTransform =
                    new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
            }

            base.OnOrientationChanged(e);
        }
        

        #region Camera maniPulation Events
        void camera_CaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            if (e.ImageStream != null)
            {
                

                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                   var vm=this.DataContext as MainViewModel;
                    vm.SaveThumbnamil(e.ImageStream);
                });
            }
        }

        void OnButtonRelease(object sender, EventArgs e)
        {

            camera.CancelFocus();
        }

        void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void camera_CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {

            }
            //   throw new NotImplementedException();
        }

        void camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {

            // camera.
           
            
          
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
               {
                   if (e.ImageStream != null)
                   {
                       bordercap.Visibility = System.Windows.Visibility.Visible;
                       WriteableBitmap bmp = new WriteableBitmap(Convert.ToInt32(camera.Resolution.Width), Convert.ToInt32(camera.Resolution.Height));
                       if (myVar.Effect is EchoEffect)
                       {
                           bmp.SetSource(e.ImageStream);

                       }
                       else
                       {
                           int[] pixelData = new int[Convert.ToInt32(camera.Resolution.Width) * Convert.ToInt32(camera.Resolution.Height)];
                           camera.GetPreviewBufferArgb32(pixelData);
                           int[] imageData = myVar.Effect.ProcessImage(pixelData);
                           imageData.CopyTo(bmp.Pixels, 0);
                           bmp.Invalidate();
                         
                       }
                       imagecap.Source = bmp;

                       var vm = this.DataContext as MainViewModel;
                       vm.SaveImage(bmp);
                       ////Copy to WriteableBitmap
                       

                   }

               });
            //throw new NotImplementedException();
        }

        void camera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            //  camera.av
           var availresolution = (List<Size>)camera.AvailableResolutions.ToList();

            availresolution.RemoveAt(0);
           // camera.Resolution = availresolution[0];
            Dispatcher.BeginInvoke(() =>
            {
                if (availresolution != null)
                    resList.ItemsSource = availresolution;
            });

        }
        void camera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    sw.Stop();

                    timeForAutoFocus = sw.ElapsedMilliseconds;
                    sw.Reset();

                });

            }
        }
        #endregion
        
        #region Click Events
        private void EffectClick(object sender, RoutedEventArgs e)
        {
            grdOption.Visibility = System.Windows.Visibility.Collapsed;
            //  CameraHolder.Visibility = System.Windows.Visibility.Collapsed;
            bordercap.Visibility = System.Windows.Visibility.Visible;
            //int[] pixelData = new int[Convert.ToInt32(camera.PreviewResolution.Width) * Convert.ToInt32(camera.PreviewResolution.Height)];
            //camera.GetPreviewBufferArgb32(pixelData);
            //WriteableBitmap bmp = new WriteableBitmap(Convert.ToInt32(camera.PreviewResolution.Width), Convert.ToInt32(camera.PreviewResolution.Height));
            var btn = sender as RadioButton;
            if (btn != null)
            {
                var content = btn.Content.ToString();
                if (content.Equals(Grayscale.Content.ToString()))
                {
                    myVar = new EffectDetails() { Effect = new GrayScaleEffect(), IsSelected = true, Name = "GrayScaleEffect" };


                }
                if (content.Equals(negative.Content.ToString()))
                {
                    myVar = new EffectDetails() { Effect = new NegativeEffect(), IsSelected = true, Name = "NegativeEffect" };
                }
                if (content.Equals(sepia.Content.ToString()))
                {
                    myVar = new EffectDetails() { Effect = new SepiaEffect(), IsSelected = true, Name = "SepiaEffect" };
                }
                //if (content.Equals(none.Content.ToString()))
                //{
                //    myVar = new EffectDetails() { Effect = new EchoEffect(), IsSelected = true, Name = "EchoEffect" };
                //}
            }
            StartRendering();
        }

     

        private void EffectsButton_Click(object sender, RoutedEventArgs e)
        {
            StartRendering();
            prouduceFrames = true;
            bordercap.Visibility = System.Windows.Visibility.Visible; 
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageList.SelectedItem != null)
            {
                if (camera.IsFocusSupported)
                {
                    camera.CancelFocus();
                    prouduceFrames = false;
                    camera.Dispose();
                    camera = null;
                }
                ImageInstance.Instance.SelectedImage = (BitmapImage)ImageList.SelectedItem;
                NavigationService.Navigate(new Uri("/Views/ImageView.xaml", UriKind.Relative));
            }
        }

        private void Resolution_Click(object sender, RoutedEventArgs e)
        {
            grdFocus.Visibility = System.Windows.Visibility.Collapsed;
            grdOption.Visibility = System.Windows.Visibility.Collapsed;
            grdEffects.Visibility = System.Windows.Visibility.Collapsed;
            grdResolution.Visibility = System.Windows.Visibility.Visible;

        }

        private void Focus_Click(object sender, RoutedEventArgs e)
        {
            grdResolution.Visibility = System.Windows.Visibility.Collapsed;
            grdFocus.Visibility = System.Windows.Visibility.Visible;
            grdEffects.Visibility = System.Windows.Visibility.Collapsed;
            grdOption.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OptionClick(object sender, RoutedEventArgs e)
        {

            var btn = sender as RadioButton;
            if (btn.Content.ToString() == "Auto")
            {
                if (camera != null)
                {
                    if (camera.IsFlashModeSupported(FlashMode.Auto))
                    {
                        camera.FlashMode = FlashMode.Auto;
                    }
                }
            }
            else if (btn.Content.ToString() == "Off")
            {
                if (camera != null)
                {
                    if (camera.IsFlashModeSupported(FlashMode.Off))
                    {
                        camera.FlashMode = FlashMode.Off;
                    }
                }
            }
            else if (btn.Content.ToString() == "On")
            {
                if (camera != null)
                {
                    if (camera.IsFlashModeSupported(FlashMode.On))
                    {
                        camera.FlashMode = FlashMode.On;
                    }
                }
            }
            else if (btn.Content.ToString() == "Red Eye Reduction")
            {
                if (camera != null)
                {
                    if (camera.IsFlashModeSupported(FlashMode.RedEyeReduction))
                    {
                        camera.FlashMode = FlashMode.RedEyeReduction;
                    }
                    else
                    {
                        camera.FlashMode = FlashMode.Auto;
                    }
                }
            }
            //   grdOption.Visibility = System.Windows.Visibility.Collapsed;
            //  VisualStateManager.GoToState(this, "ClosedPanel_Effects", true);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            prouduceFrames = false;
        //    none.IsChecked = true;
            myVar = new EffectDetails() { Effect = new EchoEffect(), IsSelected = true, Name = "EchoEffect" };
            CameraHolder.Visibility = System.Windows.Visibility.Visible;
            bordercap.Visibility = System.Windows.Visibility.Collapsed; 
            grdResolution.Visibility = System.Windows.Visibility.Collapsed;
            grdFocus.Visibility = System.Windows.Visibility.Collapsed;
            grdEffects.Visibility = System.Windows.Visibility.Collapsed;
            grdOption.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShutterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prouduceFrames = false;
                camera.CaptureImage();
            }
            catch (Exception ex) { }
        }

        private void Flash_Click(object sender, RoutedEventArgs e)
        {
            grdResolution.Visibility = System.Windows.Visibility.Collapsed;
            grdFocus.Visibility = System.Windows.Visibility.Collapsed;
            grdEffects.Visibility = System.Windows.Visibility.Collapsed;
            //grdOption.Visibility = System.Windows.Visibility.Visible;
        }
        private void AutoFocus_Click(object sender, RoutedEventArgs e)
        {
            if (camera.IsFocusSupported)
            {
                try
                {
                    sw.Start();
                    camera.Focus();
                }
                catch (Exception exe) { }
            }
        }

        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            if (camera.IsFocusAtPointSupported)
            {
                sw.Start();
                camera.FocusAtPoint(0.9,.2);
            }

        }
        private void reslou_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null)
            {
                if (btn.Tag != null)
                {
                    var data = (Size)btn.Tag;
                    camera.Resolution = data;
                }
            }
        }
        #endregion

        #region methods
        void ProduceImage()
        {

            int[] ARGBPx = new int[640 * 480];

            try
            {
                PhotoCamera phCam = camera;

                while (prouduceFrames)
                {
                    captureEvent.WaitOne();

                    pauseFramesEvent.WaitOne();
                    //Copies the current viewfinder frame into a buffer for further manipulation
                   
                    //Conversion to Seleceted Effect

                    
                    phCam.GetPreviewBufferArgb32(ARGBPx);
                    pauseFramesEvent.Reset();
                    Deployment.Current.Dispatcher.BeginInvoke(delegate()
                    {
                        //Copy to WriteableBitmap
                        if (myVar.Effect is EchoEffect)
                        {
                            ARGBPx.CopyTo(wb.Pixels, 0); this.imagecap.Source = wb;
                        }
                        else
                        {
                           
                            int[] imageData = myVar.Effect.ProcessImage(ARGBPx);
                            imageData.CopyTo(wb.Pixels, 0);
                        }
                        
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
            prouduceFrames = true;
            wb = new WriteableBitmap(640, 480);
            ImageProductionThread = new System.Threading.Thread(ProduceImage);
            this.imagecap.Source = wb;
            ImageProductionThread.Start();
        }
        #endregion

        private void CameraHolder_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if ((bool)Manual.IsChecked)
            {
                if (camera.IsFocusAtPointSupported)
                {
                    //sw.Start();
                    //camera.FocusAtPoint(e.TotalManipulation.Translation.X, e.TotalManipulation.Translation.Y);
                }
            }
        }

    }
    public class EffectDetails
    {
        /// <summary>
        /// Effect displayed name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image processing effect
        /// </summary>
        public IEffect Effect { get; set; }

        /// <summary>
        /// Gets whether this effect is selected
        /// </summary>
        public bool IsSelected { get; set; }
    }
}