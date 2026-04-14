using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.DTO;

namespace Explosion.API.Services
{
    public class UserServ
    {
        private readonly UsersRep _repository;

        public UserServ(UsersRep repository)
        {
            _repository = repository;
        }
        public List<User> ListEm()
        {
            return _repository.ListEmU();
        }
        public User? SearchId(int id)
        {
            return _repository.SearchUserId(id);
        }
        public User Create(userDTO dto)
        {
            var User = new User
            {
                Email = dto.Email,
                Endereco = dto.Endereco,
                Nome = dto.Nome,
            };
            return _repository.Create(User);
        }
        public User? Update(int id, userDTO dto)
        {
            var User = _repository.SearchUserId(id);
            if (User == null) return null;

            User.Nome = dto.Nome;
            User.Email = dto.Email;
            User.Endereco = dto.Endereco;
        
            return _repository.Update(User);
        }
        public bool Remove(int id)
        {
            var User = _repository.SearchUserId(id);
            if(User == null) return false;
            
            _repository.DeleteUser(id);
            return true;
        }
    }
}
