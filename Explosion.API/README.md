# Explosion.API

API REST para gerenciamento de produtos, usuarios, autenticacao JWT, carrinho e favoritos.

## Tecnologias

- ASP.NET Core (`net10.0`)
- Entity Framework Core
- PostgreSQL (`Npgsql`)
- JWT Bearer Authentication
- Swagger (OpenAPI)

## Funcionalidades

- Cadastro e login de usuarios
- Controle de perfil por role (`User` e `Admin`)
- CRUD de produtos
- Carrinho por usuario autenticado
- Checkout com validacao de estoque
- Favoritos por usuario autenticado

## Requisitos

- .NET SDK 10
- PostgreSQL em execucao

## Configuracao

### 1. Connection String

Arquivo: `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=explosiondb;Username=postgres;Password=...."
}
```

Ajuste para o seu ambiente.

### 2. JWT

A aplicacao exige chave JWT com no minimo 32 caracteres.

Em desenvolvimento, existe chave em `appsettings.Development.json`:

```json
"Jwt": {
  "Key": "DEV_ONLY_JWT_KEY_CHANGE_IN_PROD_1234567890"
}
```

Em producao, configure obrigatoriamente via variavel de ambiente:

- `JWT__KEY` (preferencial)
- ou `JWT_KEY`

Exemplo (PowerShell):

```powershell
$env:JWT__KEY="SUA_CHAVE_JWT_SUPER_SECRETA_COM_32+_CARACTERES"
```

## Banco de dados e migracoes

No diretorio `Explosion.API`:

```powershell
dotnet ef database update
```

Se necessario, instale a ferramenta global:

```powershell
dotnet tool install --global dotnet-ef
```

## Executando a API

No diretorio `Explosion.API`:

```powershell
dotnet restore
dotnet run
```

URLs locais (launchSettings):

- `http://localhost:5076`
- `https://localhost:7166`

Swagger (somente em `Development`):

- `http://localhost:5076/swagger`
- `https://localhost:7166/swagger`

## Autenticacao e autorizacao

A API usa `Bearer Token` (JWT).

Fluxo basico:

1. Fazer login em `POST /api/auth/login`
2. Receber `token`
3. Enviar no header:

```http
Authorization: Bearer SEU_TOKEN
```

Roles:

- Endpoints de `users` exigem `Admin`
- Criar/editar/remover produto exige `Admin`
- Comprar produto aceita `User` e `Admin`
- Carrinho e favoritos exigem usuario autenticado

## Endpoints

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`

### Products

- `GET /api/products`
- `GET /api/products/productslist` (alias da listagem)
- `GET /api/products/{id}`
- `GET /api/products/name/{name}`
- `POST /api/products` (`Admin`)
- `POST /api/products/createproduct` (`Admin`, alias)
- `PUT /api/products/{id}` (`Admin`)
- `PUT /api/products/update/{id}` (`Admin`, alias)
- `DELETE /api/products/{id}` (`Admin`)
- `DELETE /api/products/delete/{id}` (`Admin`, alias)
- `POST /api/products/{id}/buy?quantity=2` (`User` ou `Admin`)
- `POST /api/products/{id}/comprar?quantidade=2` (`User` ou `Admin`, alias)

### Cart (`[Authorize]`)

- `GET /api/cart`
- `POST /api/cart/items`
- `PUT /api/cart/items/{itemId}`
- `DELETE /api/cart/items/{itemId}`
- `DELETE /api/cart/clear`
- `POST /api/cart/checkout`

### Favorites (`[Authorize]`)

- `GET /api/favorites`
- `POST /api/favorites/{productId}`
- `DELETE /api/favorites/{productId}`

### Users (`[Authorize(Roles = "Admin")]`)

- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`
- `PATCH /api/users/promote/{id}`
- `PATCH /api/users/promover/{id}` (alias)

## Exemplos rapidos

### Registrar usuario

```bash
curl -X POST http://localhost:5076/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Enzo",
    "email": "enzo@email.com",
    "password": "123456"
  }'
```

### Login

```bash
curl -X POST http://localhost:5076/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "enzo@email.com",
    "password": "123456"
  }'
```

### Criar produto (Admin)

```bash
curl -X POST http://localhost:5076/api/products \
  -H "Authorization: Bearer SEU_TOKEN_ADMIN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Camiseta Explosion",
    "price": 99.9,
    "image": "https://url-da-imagem",
    "stock": 50,
    "description": "Modelo premium"
  }'
```

### Adicionar item no carrinho

```bash
curl -X POST http://localhost:5076/api/cart/items \
  -H "Authorization: Bearer SEU_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": 1,
    "quantity": 2
  }'
```

## Estrutura principal

```text
Controllers/
Data/
DTOs/
Models/
Repositories/
Services/
Migrations/
Program.cs
```

## Observacoes

- `UseHttpsRedirection` e aplicado somente fora de `Development`.
- O CORS usa policy `FrontendPolicy` com origens em `Cors:AllowedOrigins`.
- O serializador JSON usa `camelCase` nas respostas.

