/*
 Programmer: Duane Cressman
 Date Created: November 2019 
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SlidingTilesGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Launch_Page : Page
    {
        public ObservableCollection<StorageFile> Images { get; } = new ObservableCollection<StorageFile>();

        public Launch_Page()
        {
            this.InitializeComponent();
        }


        //move to the next page if the camera button is clicked
        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            Constants.CameraOrFile = 1;
            this.Frame.Navigate(typeof(MainPage));
        }

        //move to the next page if the file button is clicked
        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            Constants.CameraOrFile = 2;
            this.Frame.Navigate(typeof(MainPage));
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Constants.CameraOrFile = 3;
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
