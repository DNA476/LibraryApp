using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LibraryEntities context = new LibraryEntities();
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            var ctx = new LibraryEntities();
            ctx.Books.Load();
            ctx.Readers.Load();

            Application.Current.Resources["Books"] = ctx.Books.Local;
            Application.Current.Resources["Readers"] = ctx.Readers.Local;
        }


    }
}
    
        
 

