using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services {
    public class SeedService : ISeedService {
        private readonly ISeedRepository _repo;

        public SeedService (ISeedRepository repo) {
            _repo = repo;
        }

        public void SeedDatabase () {
            _repo.SeedDatabase ();
        }
    }
}