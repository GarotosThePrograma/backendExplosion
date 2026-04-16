# Regression Checklist

## 1. Auth
- `POST /api/auth/register` with a new email: expect `200`.
- Repeat same email: expect `400`.
- `POST /api/auth/login` valid credentials: expect `200` with `token`.
- `POST /api/auth/login` invalid credentials: expect `401`.

## 2. Products
- `GET /api/products`: expect `200` list.
- `GET /api/products/{id}` existing: expect `200`.
- `GET /api/products/{id}` missing: expect `404`.
- `POST /api/products` as Admin: expect `200`.
- `PUT /api/products/{id}` as Admin: expect `200`.
- `DELETE /api/products/{id}` as Admin: expect `200`.
- `POST /api/products/{id}/buy?quantity=1` as User/Admin: expect `200` or `400` if stock is insufficient.
- Legacy aliases still valid:
  - `GET /api/products/productslist`
  - `POST /api/products/createproduct`
  - `PUT /api/products/update/{id}`
  - `DELETE /api/products/delete/{id}`
  - `POST /api/products/{id}/comprar?quantidade=1`

## 3. Users (Admin only)
- `GET /api/users`: expect `200`.
- `GET /api/users/{id}` existing: expect `200`.
- `POST /api/users`: expect `200`.
- `PUT /api/users/{id}`: expect `200`.
- `DELETE /api/users/{id}`: expect `200`.
- `PATCH /api/users/promote/{id}` and `/promover/{id}`: expect `200`.
- Validate response does not expose `password`.

## 4. Cart (authenticated)
- `GET /api/cart`: expect `200`.
- `POST /api/cart/items` valid product and quantity: expect `200`.
- `PUT /api/cart/items/{itemId}` with valid quantity: expect `200`.
- `DELETE /api/cart/items/{itemId}`: expect `200`.
- `DELETE /api/cart/clear`: expect `200`.
- `POST /api/cart/checkout` with items and stock: expect `200`.
- `POST /api/cart/checkout` empty cart: expect `400`.

## 5. Favorites (authenticated)
- `GET /api/favorites`: expect `200`.
- `POST /api/favorites/{productId}` existing product: expect `200`.
- Repeat add same product: expect `200` (idempotent).
- `DELETE /api/favorites/{productId}` existing: expect `200`.
- `DELETE /api/favorites/{productId}` missing: expect `404`.

## 6. Error payload consistency
- For `400`, `404`, `500` responses, ensure body pattern is:
  - `{ "message": "..." }`
