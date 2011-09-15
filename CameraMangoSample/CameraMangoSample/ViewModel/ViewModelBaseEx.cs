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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;


namespace CameraMangoSample.ViewModel
{
    public class ViewModelBaseEx : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged1;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged1 != null)
            {
                PropertyChanged1(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
        protected void SendGoBackRequestMessage()
        {
            Messenger.Default.Send<object>(null, "GoBackRequest");
        }

        protected void SendNavigationRequestMessage(Uri uri)
        {
            Messenger.Default.Send<Uri>(uri, "NavigationRequest");
        }

       

        public virtual void TemporaryCleanup()
        {
            //This one has nothing to do
        }
    }
}
