using System.Windows;

namespace BasitNotTakip
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var wnd = new Views.MainWindow();
            wnd.Show();
        }
    }
}
