using System.Xml;
using Explosion.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserServ
{
    private readonly UserRep _repository;

    public UserServ(UserRep repository)
    {
        repository = _repository;
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
            IdUser = dto.IdUser,
            Nome = dto.Nome,
        };
        return _repository.Create(User);
    }
    public User? Update(int id, userDTO dto)
    {
        var User = _repository.SearchUserId(id);
        if (User == null) return null;

        User.Nome = dto.Nome;
        User.IdUser = dto.IdUser;
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