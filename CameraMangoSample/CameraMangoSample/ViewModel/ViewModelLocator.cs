
using CameraMangoSample.ViewModel;
namespace CameraMangoSample.ViewModel
{


    public class ViewModelLocator
    {        
        public static MainViewModel main;
        
        public ViewModelLocator()
        {            
        }

        #region plumbing for MainViewModel
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public static MainViewModel MainStatic
        {
            get
            {
                if (main == null)
                {
                    CreateMain();
                }

                return main;
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return MainStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Main property.
        /// </summary>
        public static void ClearPanorama()
        {
            if (main != null)
            {
                main.Cleanup();
                main = null;
            }
        }

        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void CreateMain()
        {
            if (main == null)
            {
                main = new MainViewModel();
            }
        }

        #endregion

        
        public static void Cleanup()
        {            
            
        }
    }

}
