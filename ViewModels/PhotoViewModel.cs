using JulMar.Windows.Mvvm;
using Quick_Photo_Viewer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Quick_Photo_Viewer
{
  internal class PhotoViewModel : INotifyPropertyChanged
  {

    #region private members

    private Photo _currentPhoto;
    private string _imageDirectory;
    private int _imageNumber;
    private int _imageCount;
    private List<string> _imageFiles;
    private PhotoCollection _photos;
    private Stretch _photoDisplayMode;

    #endregion

    #region public properties

    public Photo CurrentPhoto
    {
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
    }
    public string ImageDirectory
    {
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
    public int ImageNumber
    {
      get { return _imageNumber; }
      set { _imageNumber = value; }
    }
    public int ImageCount
    {
      get { return _imageCount; }
      set { _imageCount = value; }
    }
    public List<string> ImageFiles
    {
      get { return _imageFiles; }
      set { _imageFiles = value; }
    }
    public PhotoCollection Photos
    {
      get { return _photos; }
      set { _photos = value; }
    }
    public Stretch PhotoDisplayMode
    {
      get { return _photoDisplayMode; }
      set
      {
        if (_photoDisplayMode != value)
        {
          _photoDisplayMode = value;
          NotifyPropertyChanged("PhotoDisplayMode");
          var handler = PropertyChanged;
          if (handler != null)
          {
            handler(this, new PropertyChangedEventArgs("PhotoDisplayMode"));
          }
        }
      }
    }

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
      _photoDisplayMode = Stretch.Uniform;

      registerCommands();

      //Task.Run(() =>
      //  {
      if (Environment.GetCommandLineArgs().Length > 1)
        loadImages(Environment.GetCommandLineArgs()[1]);
      else
        //LoadImages("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\3r4en.jpg");
        loadImages("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
      //});
    }

    #endregion

    #region public methods

    #region commands

    public void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
      if (Environment.GetCommandLineArgs().Length > 1)
        loadImages(Environment.GetCommandLineArgs()[1]);
      else
        //LoadImages("C:\\Users\\Ryan\\Pictures\\sky, space and landscapes\\3r4en.jpg");
        loadImages("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
    }

    public void TranslateImage(double x, double y)
    {
      BitmapSource img = (BitmapSource)(CurrentPhoto.Image);

      CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
      CurrentPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new TranslateTransform(x, y)));

      //thePhoto.Source = currPhoto.Image;
    }

    public ICommand RotateImageLeftCommand { get; set; }
    public ICommand RotateImageRightCommand { get; set; }
    private void rotateImage(double angle)
    {
      BitmapSource img = (BitmapSource)(CurrentPhoto.Image);

      CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
      CurrentPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new RotateTransform(angle)));
    }

    public ICommand NextImageCommand { get; set; }
    private void nextImage()
    {
      ImageNumber = (ImageNumber + 1) % ImageCount;
      CurrentPhoto.ChangeImageSource(ImageFiles[ImageNumber]);
    }

    public ICommand PreviousImageCommand { get; set; }
    private void previousImage()
    {
      int num = (ImageNumber - 1) % ImageCount;
      ImageNumber = num < 0 ? ImageCount - 1 : num;
      CurrentPhoto.ChangeImageSource(ImageFiles[ImageNumber]);
    }

    public ICommand ToggleImageSizeCommand { get; set; }
    private void toggleImageSize()
    {
      if (PhotoDisplayMode == Stretch.Uniform)
      {
        PhotoDisplayMode = Stretch.None;
        //TranslateImage(thePhoto.Source.Width / 2, thePhoto.Source.Height / 2);
        //thePhoto.Cursor = Cursors.Hand;
      }
      else
      {
        PhotoDisplayMode = Stretch.Uniform;
        //thePhoto.Cursor = Cursors.Arrow;
      }
    }

    #endregion

    #endregion

    #region private helpers

    private void loadImages(string filename)
    {
      CurrentPhoto = new Photo(filename);
      //thePhoto.Source = currPhoto.Image; // new Bitmap(filename, false);
      int end = filename.LastIndexOf('\\');
      _imageDirectory = filename.Substring(0, end + 1);
      int count = 0;
      _imageFiles = new List<string>();
      foreach (string file in Directory.GetFiles(_imageDirectory))
      {
        if (isValidImage(file))
        {
          _imageFiles.Add(file);
          count++;
        }

        if (file.Equals(filename))
          _imageNumber = count - 1;

      }

      _imageCount = count;
      // images.Sort();
      //MessageBox.Show(photos.Count + "");
      _photos.Path = _imageDirectory;
      //MessageBox.Show(_photos.Count + "");
    }
    private bool isValidImage(string filename)
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
    private void registerCommands()
    {
      this.NextImageCommand = new DelegateCommand(
        new Action(() => { this.nextImage(); })
        );
      this.PreviousImageCommand = new DelegateCommand(
        new Action(() => { this.previousImage(); })
        );
      this.RotateImageLeftCommand = new DelegateCommand(
        new Action(() => { this.rotateImage(270); })
        );
      this.RotateImageRightCommand = new DelegateCommand(
        new Action(() => { this.rotateImage(90); })
        );
      this.ToggleImageSizeCommand = new DelegateCommand(
        new Action(() => { this.toggleImageSize(); })
        );
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
