using Microsoft.EntityFrameworkCore;

public class ExpDbContext : DbContext{
    public DbSet<Product> Products{get;set;}
    public DbSet<User> User{get;set;}
}