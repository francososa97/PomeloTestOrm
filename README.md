# PomeloTestOrm

# Ejemplo CRUD con Pomelo ORM y .NET 8

Este proyecto es un ejemplo básico de cómo usar Pomelo.EntityFrameworkCore.MySql como ORM en una aplicación ASP.NET Core 8 para realizar operaciones CRUD sobre una entidad `Producto`.

## Requisitos
- .NET 8 SDK
- MySQL Server

## Configuración

1. **Clona el repositorio o descarga el código.**

2. **Instala los paquetes NuGet necesarios:**
   - Pomelo.EntityFrameworkCore.MySql
   - Microsoft.EntityFrameworkCore.Design

   Ya están incluidos en el proyecto, pero si necesitas reinstalar:
   ```sh
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   dotnet add package Microsoft.EntityFrameworkCore.Design
   ```

3. **Configura la cadena de conexión en `PomeloTestOrm/appsettings.json`:**
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;port=3306;database=PomeloTestOrmDb;user=root;password=TU_PASSWORD_AQUI;"
   }
   ```
   Cambia `user` y `password` por los de tu servidor MySQL.

4. **Crea la base de datos vacía en MySQL:**
   ```sql
   CREATE DATABASE PomeloTestOrmDb;
   ```

5. **Crea y aplica la migración inicial:**
   ```sh
   dotnet ef migrations add InitialCreate --project PomeloTestOrm/PomeloTestOrm.csproj
   dotnet ef database update --project PomeloTestOrm/PomeloTestOrm.csproj
   ```

6. **Ejecuta la aplicación:**
   ```sh
   dotnet run --project PomeloTestOrm/PomeloTestOrm.csproj
   ```

## Endpoints CRUD

- `GET    /api/productos`           → Lista todos los productos
- `GET    /api/productos/{id}`      → Obtiene un producto por ID
- `POST   /api/productos`           → Crea un producto (JSON: {"nombre": "...", "precio": ...})
- `PUT    /api/productos/{id}`      → Actualiza un producto
- `DELETE /api/productos/{id}`      → Elimina un producto

Puedes probar los endpoints con Postman, curl o Swagger (si lo agregas).

## Estructura principal
- `Models/Producto.cs`        → Entidad de ejemplo
- `Data/AppDbContext.cs`     → DbContext configurado para MySQL
- `Controllers/ProductosController.cs` → API CRUD
- `Program.cs`               → Configuración de servicios y middlewares

---

¿Dudas o problemas? ¡Abre un issue o pregunta!