using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services {
    public interface ISeedService {
        void SeedDatabase ();
    }
}