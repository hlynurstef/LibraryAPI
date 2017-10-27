using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IBooksRepository
    {
         IEnumerable<BookDTOLite> GetBooks();

         BookDTOLite AddBook(BookView book);

         BookDTO GetBookById(int id);

         void DeleteBookById(int id);
         void UpdateBookById(int id, BookView book);
         IEnumerable<BookDTOLite> GetBooksByUserId(int userId);
    }

}