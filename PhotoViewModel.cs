using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Quick_Photo_Viewer
{
  internal class PhotoViewModel
  {

    #region private members

    private Photo _currentPhoto;
    private string _imageDirectory;
    private int _imageNumber;
    private int _imageCount;
    private List<string> _imageFiles;
    private PhotoCollection _photos;

    #endregion

    #region public properties

    public Photo CurrentPhoto { 
      get { return _currentPhoto; } 
      set 
      {
        if (_currentPhoto != value)
        {
          _currentPhoto = value;
          NotifyPropertyChanged("CurrentPhoto");
          var handler = PropertyChanged;
          if (handler != null)
          {
            handler(this, new PropertyChangedEventArgs("CurrentPhoto"));
          }
        }
      }
    }//new Photo("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\CrkWD.jpg");
    //Photo oldPhoto = null;
    public string ImageDirectory { 
      get { return _imageDirectory; } 
      set 
      {
        if (_imageDirectory != value)
        {
          _imageDirectory = value;
          NotifyPropertyChanged();
        }
      }
    }
    public int ImageNumber { 
      get { return _imageNumber; } 
      set { _imageNumber = value; } }
    public int ImageCount { 
      get { return _imageCount; } 
      set { _imageCount = value; } }
    public List<string> ImageFiles { 
      get { return _imageFiles; }
      set { _imageFiles = value; } }
    public PhotoCollection Photos { 
      get { return _photos; } 
      set { _photos = value; } }

    #endregion

    #region construction

    public PhotoViewModel()
    {
      _currentPhoto = null;
      _imageDirectory = null;
      _imageNumber = 0;
      _imageCount = 0;
      _imageFiles = new List<string>();
      _photos = new PhotoCollection();

      if (Environment.GetCommandLineArgs().Length > 1)
        LoadImages(Environment.GetCommandLineArgs()[1]);
      else
        //LoadImages("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\3r4en.jpg");
        LoadImages("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
    }

    #endregion

    #region public methods

    #region commands

    public void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
      if (Environment.GetCommandLineArgs().Length > 1)
        LoadImages(Environment.GetCommandLineArgs()[1]);
      else
        //LoadImages("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\3r4en.jpg");
        LoadImages("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
    }

    public void TranslateImage(double x, double y)
    {
      BitmapSource img = (BitmapSource)(CurrentPhoto.Image);

      CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
      CurrentPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new TranslateTransform(x, y)));

      //thePhoto.Source = currPhoto.Image;
    }

    #endregion



    public void RotateImage(double angle)
    {

      BitmapSource img = (BitmapSource)(CurrentPhoto.Image);

      CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
      CurrentPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new RotateTransform(angle)));

      //thePhoto.Source = currPhoto.Image;
    }
    #endregion

    #region private helpers

    private void LoadImages(string filename)
    {
      CurrentPhoto = new Photo(filename);
      //thePhoto.Source = currPhoto.Image; // new Bitmap(filename, false);
      int end = filename.LastIndexOf('\\');
      _imageDirectory = filename.Substring(0, end + 1);
      int count = 0;
      _imageFiles = new List<string>();
      foreach (string file in Directory.GetFiles(_imageDirectory))
      {
        if (IsValidImage(file))
        {
          _imageFiles.Add(file);
          count++;
        }

        if (file.Equals(filename))
          _imageNumber = count - 1;

      }

      // images.Sort();
      //MessageBox.Show(photos.Count + "");
      _photos.Path = _imageDirectory;
      //MessageBox.Show(_photos.Count + "");
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

    #endregion

    #region notification
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

  }
}
