using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IBooksService
    {
        BookDTOLite AddBook(BookView book);
        IEnumerable<BookDTOLite> GetBooks();

        BookDTOLite GetBookById(int id);
        
        BookDTOLite DeleteBookById(int id);

        BookDTOLite EditBookById(int id, BookView book);

        IEnumerable<BookDTOLite> GetBooksByUserId(int userId);
    }
}