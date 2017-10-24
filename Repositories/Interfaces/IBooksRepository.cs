using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IBooksRepository
    {
         IEnumerable<BookDTO> GetBooks();

         BookDTO AddBook(BookView book);

         BookDTO GetBookById(int id);

         BookDTO DeleteBookById(int id);
         BookDTO EditBookById(int id, BookView book);
    }

}