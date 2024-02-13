using Books.Data;
using Books.Models;

namespace BookStore.Services
{
    public class AuthorService
    {
        public Author FindOrCreateAuthor(string name, BookContext db)
        {
            var existingAuthor = db.Authors.FirstOrDefault(a => a.Name == name);

            if (existingAuthor != null)
            {
                return existingAuthor;
            }
            else
            {
                var newAuthor = new Author { Name = name };
                db.Authors.Add(newAuthor);
                db.SaveChanges();

                return newAuthor;
            }
        }
    }
}
