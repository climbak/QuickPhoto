using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Quick_Photo_Viewer.Models
{
  /// <summary>
  /// This class describes a single photo - its location, the image and 
  /// the metadata extracted from the image.
  /// </summary>
  internal class Photo : INotifyPropertyChanged
  {
    public Photo(string path)
    {
      _path = path;
      _source = new Uri(path);
      loadImageFromDisk(_source);
    }

    public override string ToString()
    {
      return _source.ToString();
    }

    private string _path;

    private Uri _source;
    public string Source { get { return _path; } }

    private BitmapFrame _image;
    public BitmapFrame Image
    {
      get { return _image; }
      set
      {
        if (_image != value)
        {
          _image = value;
          NotifyPropertyChanged("Image");
        }
      }
    }

    public void ChangeImageSource(string path)
    {
      _source = new Uri(path);
      loadImageFromDisk(_source);
    }

    private ExifMetadata _metadata;
    public ExifMetadata Metadata
    {
      get { return _metadata; }
      set
      {
        if (_metadata != value)
        {
          _metadata = value;
          NotifyPropertyChanged("Metadata");
        }
      }
    }

    private void loadImageFromDisk(Uri path)
    {
      Image = BitmapFrame.Create(path);
      Metadata = new ExifMetadata(path);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
