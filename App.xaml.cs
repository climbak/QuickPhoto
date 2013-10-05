using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quick_Photo_Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      protected override void OnStartup(StartupEventArgs e)
      {
        base.OnStartup(e);

        var app = new PhotoView();
        //var context = new PhotoViewModel();
        //app.DataContext = context;
        app.Show();
      }
    }
}
