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
            return _languageRepository.GetAll();
        }

        public Language? GetById(Guid ID)
        {
            if (ID == Guid.Empty)
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

        public void Create(string Name)
        {
            var language = new Language
            {
                ID = Guid.NewGuid(),
                Name = Name,
                LanguageID = Guid.NewGuid(),

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _languageRepository.Add(language);
            Console.WriteLine("Idioma adicionado com sucesso!");
        }


        public void Update(Guid ID, string Name)
        {
            var existinglanguage = _languageRepository.GetById(ID);
            if (existinglanguage == null)
            {
                Console.WriteLine("Idioma não encontrado.");
                return;
            }

            existinglanguage.Name = Name;
            
            existinglanguage.UpdatedAt = DateTime.Now;

            _languageRepository.Update(existinglanguage);
            Console.WriteLine("Idioma atualizado com sucesso!");
        }

        public void Delete(Guid ID)
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
    }
}
