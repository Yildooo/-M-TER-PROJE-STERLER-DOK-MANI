using System.Windows;
using System.Linq;
using BasitNotTakip.Data;
using BasitNotTakip.Models;

namespace BasitNotTakip.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Database.Load();
            RefreshLists();
        }

        private void RefreshLists()
        {
            NotesList.ItemsSource = null;
            NotesList.ItemsSource = Database.Notlar.OrderBy(n => n.Id).ToList();

            TasksList.ItemsSource = null;
            TasksList.ItemsSource = Database.Gorevler.OrderBy(g => g.Id).ToList();
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            var baslik = NoteTitle.Text?.Trim();
            var icerik = NoteContent.Text?.Trim();
            if (string.IsNullOrEmpty(baslik) && string.IsNullOrEmpty(icerik)) return;

            var n = new Not { Baslik = baslik, Icerik = icerik };
            Database.AddNot(n);
            NoteTitle.Text = string.Empty;
            NoteContent.Text = string.Empty;
            RefreshLists();
        }

        private void RemoveNote_Click(object sender, RoutedEventArgs e)
        {
            if (NotesList.SelectedItem is Not n)
            {
                Database.RemoveNot(n.Id);
                RefreshLists();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var desc = TaskDescription.Text?.Trim();
            if (string.IsNullOrEmpty(desc)) return;
            var g = new Gorev { Aciklama = desc, Tamamlandi = false };
            Database.AddGorev(g);
            TaskDescription.Text = string.Empty;
            RefreshLists();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksList.SelectedItem is Gorev g)
            {
                Database.RemoveGorev(g.Id);
                RefreshLists();
            }
        }

        private void ToggleTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksList.SelectedItem is Gorev g)
            {
                Database.ToggleGorev(g.Id);
                RefreshLists();
            }
        }
    }
}
