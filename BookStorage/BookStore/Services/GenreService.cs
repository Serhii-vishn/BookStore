using Books.Data;
using Books.Models;

namespace BookStore.Services
{
    public class GenreService
    {
        public Genre FindOrCreateGenre(string name, BookContext db)
        {
            var existingGenre = db.Genres.FirstOrDefault(a => a.Name == name);

            if (existingGenre != null)
            {
                return existingGenre;
            }
            else
            {
                var newGenre = new Genre { Name = name };
                db.Genres.Add(newGenre);
                db.SaveChanges();

                return newGenre;
            }
        }
    }
}
