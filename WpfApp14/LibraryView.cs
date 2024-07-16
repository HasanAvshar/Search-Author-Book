using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace WpfApp14
{
    public class LibraryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Author> _authors;
        private ObservableCollection<Book> _books;
        private Author _selectedAuthor;

        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                LoadBooksByAuthor(_selectedAuthor.AuthorId);
                OnPropertyChanged();
            }
        }

        public LibraryViewModel()
        {
            Authors = new ObservableCollection<Author>();
            Books = new ObservableCollection<Book>();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            Authors.Clear(); 
            string connectionString = ConfigurationManager.ConnectionStrings["Library"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryAuthors = "SELECT Id, FirstName, LastName FROM Authors";
                SqlCommand commandAuthors = new SqlCommand(queryAuthors, connection);
                connection.Open();
                SqlDataReader reader = commandAuthors.ExecuteReader();
                while (reader.Read())
                {
                    Authors.Add(new Author { AuthorId = reader.GetInt32(0), FirstName = reader.GetString(1), LastName = reader.GetString(2) });
                }
            }
        }

        public void LoadBooksByAuthor(int authorId)
        {
            Books.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["Library"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryBooks = "SELECT Id, Name, Id_Author FROM Books WHERE Id_Author = @AuthorId";
                SqlCommand commandBooks = new SqlCommand(queryBooks, connection);
                commandBooks.Parameters.AddWithValue("@AuthorId", authorId);
                connection.Open();
                SqlDataReader reader = commandBooks.ExecuteReader();
                while (reader.Read())
                {
                    Books.Add(new Book { Id = reader.GetInt32(0), Name = reader.GetString(1), Author_Id = reader.GetInt32(2) });
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}