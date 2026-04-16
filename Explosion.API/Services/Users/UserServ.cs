using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.DTOs;

namespace Explosion.API.Services
{
    public class UserServ
    {
        private readonly UserRep _repository;

        public UserServ(UserRep repository)
        {
            _repository = repository;
        }

        public List<UserResponseDTO> List()
        {
            return _repository.List().Select(MapToResponse).ToList();
        }

        public UserResponseDTO? GetById(int id)
        {
            var user = _repository.GetById(id);
            return user is null ? null : MapToResponse(user);
        }

        public UserResponseDTO Create(UserDTO dto)
        {
            var user = new User
            {
                Email = dto.Email,
                Address = dto.Address,
                Name = dto.Name,
            };

            return MapToResponse(_repository.Create(user));
        }

        public UserResponseDTO? Update(int id, UserDTO dto)
        {
            var user = _repository.GetById(id);
            if (user == null) return null;

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Address = dto.Address;

            return MapToResponse(_repository.Update(user));
        }

        public bool Remove(int id)
        {
            var user = _repository.GetById(id);
            if (user == null) return false;

            _repository.DeleteById(id);
            return true;
        }

        private static UserResponseDTO MapToResponse(User user)
        {
            return new UserResponseDTO
            {
                Id = user.IdUser,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role
            };
        }
    }
}
