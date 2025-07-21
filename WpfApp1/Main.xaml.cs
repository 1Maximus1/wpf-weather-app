using DesktopContactsApp;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private List<Contact> contactsList;
        public Main()
        {
            InitializeComponent();

            contactsList = new List<Contact>();

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
                contactsList = conn.Table<Contact>().OrderBy(c => c.Name).ToList();
                if (contactsList.Count != 0)
                {
                    contactsListView.ItemsSource = contactsList;
                }

                //var variable = from c2 in contactsList
                //               orderby c2.Name
                //               select c2;
            }
        }


        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox searchTextBox = (TextBox)sender;

            var filteredList2 = contactsList.Where(c => c.Name.ToLower().Contains(searchTextBox.Text)).ToList();

            var filteredList = (from c2 in contactsList
                                where c2.Name.ToLower().Contains(searchTextBox.Text.ToLower())
                                orderby c2.Email
                                select c2).ToList();

            contactsListView.ItemsSource = filteredList;
        }

        private void contactsListsView_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            Contact selectedContact =  (Contact)contactsListView.SelectedItem;

            if (selectedContact != null) 
            {
                ContactDetailsWindow contactDetailsWindow = new ContactDetailsWindow(selectedContact);
                contactDetailsWindow.ShowDialog();
                ReadDataBase();
            }
        }
    }
}
