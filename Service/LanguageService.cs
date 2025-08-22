using System;
using System.Collections.Generic;
using Domain;
using Storage;

namespace Service
{
    public class LanguageService
    {
        private readonly LanguageRepository _languageRepository;

        public LanguageService()
        {
            _languageRepository = new LanguageRepository();
        }

        public List<Language> GetAll()
        {
            return (List<Language>)_languageRepository.GetAll();
        }

        public Language? GetById(int ID)
        {
            if (ID < 0)
            {
                Console.WriteLine("ID inválido.");
                return null;
            }

            var language = _languageRepository.GetById(ID);

            if (language == null)
            {
                Console.WriteLine("Idioma não encontrado.");
                return null;
            }

            return language;
        }

        public void Create(int ID, string Name, Guid LanguageID)
        {
            var language = new Language
            {
                ID = ID,
                Name = Name,

                Created_At = DateTime.Now,
                Updated_At = DateTime.Now
            };

            _languageRepository.Add(language);
            Console.WriteLine("Idioma adicionado com sucesso!");
        }


        public void Update(int ID, string Name)
        {
            var existinglanguage = _languageRepository.GetById(ID);
            if (existinglanguage == null)
            {
                Console.WriteLine("Idioma não encontrado.");
                return;
            }

            existinglanguage.Name = Name;
            
            existinglanguage.Updated_At = DateTime.Now;

            _languageRepository.Update(existinglanguage);
            Console.WriteLine("Idioma atualizado com sucesso!");
        }

        public void Delete(int ID)
        {
            var existinglanguage = _languageRepository.GetById(ID);
            if (existinglanguage == null)
            {
                Console.WriteLine("Idioma não encontrado.");
                return;
            }

            _languageRepository.Delete(ID);
            Console.WriteLine("Idioma removido com sucesso!");
        }

        internal void Create(int iD, string name, string languageID)
        {
            throw new NotImplementedException();
        }
    }
}
