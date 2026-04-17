# Explosion Backend

Backend da plataforma Explosion, construído com ASP.NET Core.

## Estrutura

- `Explosion.API/`: API principal (controllers, services, repositories, models e migrations)

## Começando

1. Acesse a pasta da API:

```powershell
cd Explosion.API
```

2. Restaure dependências e aplique migrations:

```powershell
dotnet restore
dotnet ef database update
```

3. Execute a aplicação:

```powershell
dotnet run
```

A documentação de uso (endpoints, autenticação, exemplos e configuração) está em:

- [`Explosion.API/README.md`](Explosion.API/README.md)
