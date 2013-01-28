using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quick_Photo_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Photo currPhoto = null;//new Photo("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\CrkWD.jpg");
        //Photo oldPhoto = null;
        private string imageDir = null;
        private int imageNum = 0;
        private List<string> images;
        public PhotoCollection photos = new PhotoCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                LoadImages(Environment.GetCommandLineArgs()[1]);
            }
            else
            {
                //MessageBox.Show("fail");
                LoadImages("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\3r4en.jpg");
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
           // dlg.DefaultExt = ".jpg[[|.jpeg][|.png][|.tif][|.tiff][|.gif][|.bmp][|.ico]]";
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.gif;*.bmp;*.ico";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                //MessageBox.Show(filename);
                LoadImages(filename);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Save_As_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Properties_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow();
            win.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    //e.IsInputKey = true;
                    break;
            }
        }

        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            //ImageSource old = thePhoto.Source;
            //oldPhoto = currPhoto;
            switch (e.Key)
            {
                case Key.Left:
                    imageNum = imageNum == 0 ? images.Count - 1 : imageNum - 1;
                    currPhoto = new Photo(images[imageNum]);
                    thePhoto.Source = currPhoto.Image;
                    //old.Dispose();
                    break;
                case Key.Right:
                    imageNum = (imageNum + 1) % images.Count;
                    currPhoto = new Photo(images[imageNum]);
                    thePhoto.Source = currPhoto.Image;
                    //old.Dispose();
                    break;
                case Key.Up:
                    //thePhoto.Source.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    ////thePhoto.Invalidate();
                    RotateImage(270.0);
                    break;
                case Key.Down:
                    //thePhoto.Source.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    ////thePhoto.Invalidate();
                    RotateImage(90.0);
                    break;
                case Key.Enter:
                    if (thePhoto.Stretch == System.Windows.Media.Stretch.Uniform)
                    {
                        thePhoto.Stretch = System.Windows.Media.Stretch.None;
                        TranslateImage(thePhoto.Source.Width / 2, thePhoto.Source.Height / 2);
                        thePhoto.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        thePhoto.Stretch = System.Windows.Media.Stretch.Uniform;
                        thePhoto.Cursor = Cursors.Arrow;
                    }
                    break;
            }
        }

        private void LoadImages(string filename)
        {
            currPhoto = new Photo(filename);
            thePhoto.Source = currPhoto.Image; // new Bitmap(filename, false);
            int end = filename.LastIndexOf('\\');
            imageDir = filename.Substring(0, end + 1);
            int count = 0;
            images = new List<string>();
            foreach (string file in Directory.GetFiles(imageDir))
            {
                if (IsValidImage(file))
                {
                    images.Add(file);
                    count++;
                }

                if (file.Equals(filename))
                    imageNum = count - 1;

            }

           // images.Sort();
            //MessageBox.Show(photos.Count + "");
            photos.Path = imageDir;
            MessageBox.Show(photos.Count+"");
        }

        private bool IsValidImage(string filename)
        {
            if (filename.Contains(".jpg") || filename.Contains(".jpeg"))
                return true;
            //if (filename.Contains(".gif"))
            //    return true;
            if (filename.Contains(".bmp"))
                return true;
            if (filename.Contains(".png"))
                return true;
            if (filename.Contains(".tif") || filename.Contains(".tiff"))
                return true;

            return false;
            //try
            //{
            //    Image newImage = Image.FromFile(filename);
            //}
            //catch (OutOfMemoryException ex)
            //{
            //    // Image.FromFile will throw this if file is invalid.
            //    // Don't ask me why.
            //    return false;
            //}
            //return true;
        }

        private void TranslateImage(double x, double y)
        {
            BitmapSource img = (BitmapSource)(currPhoto.Image);

            CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            currPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new TranslateTransform(x, y)));

            thePhoto.Source = currPhoto.Image;
        }

        private void RotateImage(double angle)
        {

            BitmapSource img = (BitmapSource)(currPhoto.Image);

            CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            currPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new RotateTransform(angle)));

            thePhoto.Source = currPhoto.Image;
        }

        private void ImagesExpander_Expanded_1(object sender, RoutedEventArgs e)
        {
            ImageList.Visibility = System.Windows.Visibility.Visible;
            layout.ColumnDefinitions[0].Width = new GridLength(0.25, GridUnitType.Star);
        }

        private void PropExpander_Expanded(object sender, RoutedEventArgs e)
        {
            PropList.Visibility = System.Windows.Visibility.Visible;
            layout.ColumnDefinitions[2].Width = new GridLength(0.25, GridUnitType.Star);
        }

        private void ImagesExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            ImageList.Visibility = System.Windows.Visibility.Collapsed;
            layout.ColumnDefinitions[0].Width = new GridLength(0.028, GridUnitType.Star);
        }

        private void PropExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            PropList.Visibility = System.Windows.Visibility.Collapsed;
            layout.ColumnDefinitions[2].Width = new GridLength(0.028, GridUnitType.Star);
        }

        private void viewClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
