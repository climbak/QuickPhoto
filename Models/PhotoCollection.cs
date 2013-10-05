using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Photo_Viewer.Models
{
  /// <summary>
  /// This class represents a collection of photos in a directory.
  /// </summary>
  internal class PhotoCollection : ObservableCollection<Photo>
  {
    public PhotoCollection() { }

    public PhotoCollection(string path) : this(new DirectoryInfo(path)) { }

    public PhotoCollection(DirectoryInfo directory)
    {
      _directory = directory;
      Update();
    }

    public string Path
    {
      get { return _directory.FullName; }
      set
      {
        _directory = new DirectoryInfo(value);
        Update();
      }
    }

    public DirectoryInfo Directory
    {
      get { return _directory; }
      set
      {
        _directory = value;
        Update();
      }
    }

    private void Update()
    {
      this.Clear();
      try
      {
        foreach (FileInfo f in _directory.GetFiles("*.jpg"))
        {
          //MessageBox.Show(f.FullName);
          try
          {
            Add(new Photo(f.FullName));
          }
          catch (ArgumentOutOfRangeException e)
          {
            //MessageBox.Show(e.ToString());
          }
        }
        //MessageBox.Show("after foreach: count = " + this.Count);

      }
      catch (DirectoryNotFoundException)
      {
        System.Windows.MessageBox.Show("No Such Directory");
      }
    }

    DirectoryInfo _directory;
  }
}
