using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IBooksRepository
    {
         IEnumerable<BookDTOLite> GetBooks();

         BookDTOLite AddBook(BookView book);

         BookDTOLite GetBookById(int id);

         BookDTOLite DeleteBookById(int id);
         BookDTOLite EditBookById(int id, BookView book);
         IEnumerable<BookDTOLite> GetBooksByUserId(int userId);
    }

}