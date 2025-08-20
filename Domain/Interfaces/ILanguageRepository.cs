using Domain;

namespace Biblioteca.Domain.Interfaces
{
    public interface ILanguageRepository
    {
        void Add(Language language);
        Language GetById(int ID);
        IEnumerable<Language> GetAll();
        void Update(Language language);
        void Delete(int ID);
    }
}