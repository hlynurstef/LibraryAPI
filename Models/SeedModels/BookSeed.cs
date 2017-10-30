namespace LibraryAPI.Models.SeedModels
{
    /// <summary>
    /// Class that is only used when reading json data from the file to seed the database
    /// </summary>
    public class BookSeed
    {
        public int bok_id { get; set; }
        public string bok_titill { get; set; }
        public string fornafn_hofundar { get; set; }
        public string eftirnafn_hofundar { get; set; }
        public System.DateTime utgafudagur { get; set; }
        public string ISBN { get; set; }
        
    }
}