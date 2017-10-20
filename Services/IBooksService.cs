using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IBooksService
    {
        BookDTO AddBook(BookView book);
        IEnumerable<BookDTO> GetBooks();

        BookDTO GetBookById(int id);
    }
}