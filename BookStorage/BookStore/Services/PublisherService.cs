using Books.Data;
using Books.Models;

namespace BookStore.Services
{
    public class PublisherService
    {
        public Publisher FindOrCreatePublisher(string name, BookContext db)
        {
            var existingPublisher = db.Publishers.FirstOrDefault(a => a.Name == name);

            if (existingPublisher != null)
            {
                return existingPublisher;
            }
            else
            {
                var newPublisher = new Publisher { Name = name };
                db.Publishers.Add(newPublisher);
                db.SaveChanges();

                return newPublisher;
            }
        }
    }
}
