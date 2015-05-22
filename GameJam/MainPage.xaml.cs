using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameJam
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        BitmapImage[] bi = new BitmapImage[100];
        String[] names = new string[100];
        Button[] btns = new Button[100];
        int i = 0;
        int selected = 0;

        private void next(object sender, KeyEventHandler e)
        {

        }
        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var storageFile = items[0] as StorageFile;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    if (!exists(storageFile.DisplayName))
                    {
                        bi[i] = bitmapImage;
                        names[i] = storageFile.DisplayName;
                        //RadioButton r = new RadioButton();
                        //r.Content = i.ToString();// storageFile.DisplayName;//
                        //radioContainer.Children.Add(r);
                        Button t = new Button();
                        t.Content = i.ToString();
                        t.Background = new SolidColorBrush(Colors.Aqua);
                        t.Tapped += T_Tapped;
                        btns[i] = t;
                        setImage(i);
                        radioContainer.Children.Add(t);
                        selected++;
                        i++;
                    }
                }
            }
        }

        private void T_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int a = Convert.ToInt32(((Button)sender).Content);
            setImage(a);
            selected = a;
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;

            e.DragUIOverride.Caption = "you are dragging a photo";
            //e.DragUIOverride.IsCaptionVisible = true;
            //e.DragUIOverride.IsContentVisible = true;
            //e.DragUIOverride.IsGlyphVisible = true;
            //e.DragUIOverride.SetContentFromBitmapImage(new BitmapImage(new Uri("ms-appx:///Assets/clippy.jpg")));

        }
        bool exists(string i)
        {
            foreach (string item in names)
            {
                if (item == i)
                    return true;
            }
            return false;
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Up)
                selected--;
            else if (e.Key == Windows.System.VirtualKey.Down)
                selected++;
            if (selected < 0)
                selected = 0;
            else if (selected >= i)
                selected = i-1;
            setImage(selected);
            System.Diagnostics.Debug.WriteLine(selected);
            //MessageDialog m = new MessageDialog(selected.ToString());
            //m.ShowAsync();
        }
        void setImage(int id)
        {
            DroppedImage.Source = bi[id];
            DroppedImage.Stretch = Stretch.UniformToFill;
            foreach (Button item in btns)
            {
                if (item != null)
                {
                    item.Background = new SolidColorBrush(Colors.Blue);
                }
            }
            btns[id].Background = new SolidColorBrush(Colors.Aqua);
        }
    }
}
