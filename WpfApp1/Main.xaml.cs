using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            ReadDataBase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();

            ReadDataBase();
        }

        void ReadDataBase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {

                conn.CreateTable<Contact>();
                var contacts = conn.Table<Contact>().ToList();
                if (contacts.Count != 0)
                {
                    //foreach (var contact in contacts)
                    //{
                    //    contactsListView.Items.Add(new ListViewItem()
                    //    {
                    //        Content = contact,
                    //    });
                    //}
                    contactsListView.ItemsSource = contacts;
                }
            }
        }
    }
}
