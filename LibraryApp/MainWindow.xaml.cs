using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        private LibraryEntities ctx = new LibraryEntities();

        public MainWindow()
        {
            InitializeComponent();

            ctx = new LibraryEntities();

            // загружаем данные
            ctx.Books.Load();
            ctx.Readers.Load();
            ctx.Loans.Load();

            BooksGrid.ItemsSource = ctx.Books.Local;
            ReadersGrid.ItemsSource = ctx.Readers.Local;
            LoansGrid.ItemsSource = ctx.Loans.Local;

            // биндим ComboBox в "Выдачах"
            var bookColumn = LoansGrid.Columns.OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => (string)c.Header == "Книга");
            if (bookColumn != null)
                bookColumn.ItemsSource = ctx.Books.Local;

            var readerColumn = LoansGrid.Columns.OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => (string)c.Header == "Читатель");
            if (readerColumn != null)
                readerColumn.ItemsSource = ctx.Readers.Local;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainTab.SelectedItem is TabItem tab)
            {
                if (tab.Header.ToString() == "Книги")
                {
                    ctx.Books.Add(new Books { Author = "Автор", Title = "Книга", PublishYear = DateTime.Now.Year, Price = 100, Available = 1, Annotation = "Без аннотации" });
                }
                else if (tab.Header.ToString() == "Читатели")
                {
                    ctx.Readers.Add(new Readers { FullName = "Новый читатель", Address = "Адрес", Phone = "0000000" });
                }
                else if (tab.Header.ToString() == "Выдачи")
                {
                    if (ctx.Books.Any() && ctx.Readers.Any())
                    {
                        ctx.Loans.Add(new Loans
                        {
                            CopyId = ctx.Books.First().BookId,
                            ReaderId = ctx.Readers.First().ReaderId,
                            LoanDate = DateTime.Now,
                            ReturnDate = DateTime.Now.AddMonths(1),
                            IsReturned = false
                        });
                    }
                    
                    else
                    {
                        MessageBox.Show("Нет книг или читателей для выдачи!");
                    }
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MainTab.SelectedItem is TabItem tab)
            {
                if (tab.Header.ToString() == "Книги" && BooksGrid.SelectedItem is Books b)
                    ctx.Books.Remove(b);
                else if (tab.Header.ToString() == "Читатели" && ReadersGrid.SelectedItem is Readers r)
                    ctx.Readers.Remove(r);
                else if (tab.Header.ToString() == "Выдачи" && LoansGrid.SelectedItem is Loans l)
                    ctx.Loans.Remove(l);
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ctx.SaveChanges();
                MessageBox.Show("Изменения сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}