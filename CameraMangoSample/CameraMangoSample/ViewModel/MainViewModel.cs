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
using Microsoft.Devices;
using System.Threading;
using System.Windows.Media.Imaging;
using PhotoFun.Effects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using GalaSoft.MvvmLight.Command;

namespace CameraMangoSample.ViewModel
{
    public class MainViewModel : ViewModelBaseEx
    {
      
        public MainViewModel()
        {

        }
        #region Properties
       
        #endregion
       
        #region VisibilityProperties
        Visibility grdFocus = Visibility.Collapsed;

        public Visibility GrdFocus
        {
            get { return grdFocus; }
            set { grdFocus = value; OnPropertyChanged("GrdFocus"); }
        }
        Visibility grdOptions = Visibility.Collapsed;

        public Visibility GrdOptions
        {
            get { return grdOptions; }
            set
            {
                grdOptions = value;
                OnPropertyChanged("GrdOptions");
            }
        }

        Visibility grdEffects = Visibility.Collapsed;

        public Visibility GrdEffects
        {
            get { return grdEffects; }
            set
            {
                grdEffects = value;
                OnPropertyChanged("GrdEffects");
            }
        }
        Visibility grdResolution = Visibility.Collapsed;

        public Visibility GrdResolution
        {
            get { return grdResolution; }
            set { grdResolution = value; OnPropertyChanged("GrdResolution"); }
        }
        Visibility grdImages = Visibility.Collapsed;

        public Visibility GrdImages
        {
            get { return grdImages; }
            set { grdImages = value; OnPropertyChanged("GrdImages"); }
        }

        #endregion
     
        #region Commands
        public ICommand FlashCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (GrdOptions == Visibility.Collapsed)
                    {
                        GrdOptions = Visibility.Visible;
                    }
                    else
                    {
                        GrdOptions = Visibility.Collapsed;

                    }
                    GrdFocus = Visibility.Collapsed;
                    GrdEffects = Visibility.Collapsed;
                    GrdResolution = Visibility.Collapsed; GrdImages = Visibility.Collapsed;
                });
            }
        }
        public ICommand FocusCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (GrdFocus == Visibility.Collapsed)
                    {
                        GrdFocus = Visibility.Visible;
                    }
                    else
                    {
                        GrdFocus = Visibility.Collapsed;

                    } GrdImages = Visibility.Collapsed;
                    GrdEffects = Visibility.Collapsed;
                    GrdResolution = Visibility.Collapsed; GrdOptions = Visibility.Collapsed;
                });
            }
        }
        public ICommand EffectsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (GrdEffects == Visibility.Collapsed)
                    {
                        GrdEffects = Visibility.Visible;
                    }
                    else
                    {
                        GrdEffects = Visibility.Collapsed;

                    } GrdFocus = Visibility.Collapsed;
                    GrdImages = Visibility.Collapsed;
                    GrdResolution = Visibility.Collapsed; GrdOptions = Visibility.Collapsed;
                });
            }
        }
        public ICommand ResolutionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (GrdResolution == Visibility.Collapsed)
                    {
                        GrdResolution = Visibility.Visible;
                    }
                    else
                    {
                        GrdResolution = Visibility.Collapsed;

                    } GrdFocus = Visibility.Collapsed;
                    GrdEffects = Visibility.Collapsed;
                    GrdImages = Visibility.Collapsed;
                    GrdOptions = Visibility.Collapsed;
                });
            }
        }
        public ICommand LoadImageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (GrdImages == Visibility.Collapsed)
                    {
                        GrdImages = Visibility.Visible;
                    }
                    else
                    {
                        GrdImages = Visibility.Collapsed;

                    } 
                    GrdFocus = Visibility.Collapsed;
                    GrdEffects = Visibility.Collapsed;
                 //   GrdImages = Visibility.Collapsed;
                    GrdOptions = Visibility.Collapsed;
                    Images.Clear();
                    loadImages();
                    //  GrdResolution = Visibility.Collapsed; GrdOptions = Visibility.Collapsed;
                });
            }
        }
        #endregion
        #region  Observable Properties
        ObservableCollection<BitmapImage> images= new ObservableCollection<BitmapImage>();

        public ObservableCollection<BitmapImage> Images
        {
            get { return images; }
            set { images = value; OnPropertyChanged("Images"); }
        } 
        #endregion
        internal void SaveThumbnamil(Stream stream)
        {
        //    BitmapImage bmpImage = new BitmapImage();
        //    bmpImage.CreateOptions = BitmapCreateOptions.None; // default is .DelayCreation
        //    bmpImage.SetSource(stream);
        //    IsolatedStorageFile cacheStore = IsolatedStorageFile.GetUserStoreForApplication();
        //   if(!cacheStore.DirectoryExists("Images"))
        //   {
        //       cacheStore.CreateDirectory("Images");

        //   }
         
        //    string fileNme = "Thumb" + new Random().Next() + "_th.jpg";
        //    string fileName = string.Format("Images\\{0}", fileNme);
        //    // Save it as a JPEG to isolated storage.
        //    using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create, FileAccess.Write))
        //        {
        //            WriteableBitmap bitmap = new WriteableBitmap(bmpImage);
        //            bitmap.SaveJpeg(targetStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);

        //        }
        //    }
        }

        internal void SaveImage(WriteableBitmap bmp)
        {
            IsolatedStorageFile cacheStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (!cacheStore.DirectoryExists("Images"))
            {
                cacheStore.CreateDirectory("Images");

            }

            string fileNme = "Thumb" + new Random().Next() + "_th.jpg";
            string fileName = string.Format(@"Images\{0}", fileNme);
          //  string fileName = "Image" + new Random().Next() + "_th.jpg";
            // Save it as a JPEG to isolated storage.
            using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                {
                    //WriteableBitmap bitmap = new WriteableBitmap(bmpImage);
                    bmp.SaveJpeg(targetStream, bmp.PixelWidth, bmp.PixelHeight, 0, 100);

                }
            }
        }
        internal void loadImages()
        {
            using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var files = isStore.GetFileNames(@"Images");
                foreach (var file in files)
                {
                    
                        using (IsolatedStorageFileStream targetStream = isStore.OpenFile(file, FileMode.Open, FileAccess.Read))
                        {
                            Stream str = (Stream)targetStream;
                            BitmapImage bmpImage = new BitmapImage();
                            bmpImage.CreateOptions = BitmapCreateOptions.BackgroundCreation; // default is .DelayCreation
                            bmpImage.SetSource(str);
                            Images.Add(bmpImage);
                        }
                   
                }
            }
        }
    }
}
