using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PomeloTestOrm.Data;
using PomeloTestOrm.Models;

namespace PomeloTestOrm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductosController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/productos?nombre=abc&minPrecio=10&maxPrecio=100&sort=precio_desc&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFiltered(
            string? nombre = null,
            decimal? minPrecio = null,
            decimal? maxPrecio = null,
            string? sort = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Productos.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));
            if (minPrecio.HasValue)
                query = query.Where(p => p.Precio >= minPrecio);
            if (maxPrecio.HasValue)
                query = query.Where(p => p.Precio <= maxPrecio);

            // Ordenamiento
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "precio_desc")
                    query = query.OrderByDescending(p => p.Precio);
                else if (sort == "precio")
                    query = query.OrderBy(p => p.Precio);
                else if (sort == "nombre_desc")
                    query = query.OrderByDescending(p => p.Nombre);
                else if (sort == "nombre")
                    query = query.OrderBy(p => p.Nombre);
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            // Paginación
            var productos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new { p.Id, p.Nombre, p.Precio }) // Proyección
                .ToListAsync();
            return productos;
        }

        // GET: api/productos/sql
        [HttpGet("sql")]
        public async Task<ActionResult<IEnumerable<object>>> GetWithRawSql()
        {
            var productos = await _context.Productos
                .FromSqlRaw("SELECT * FROM Productos WHERE Precio > {0}", 50)
                .Select(p => new { p.Id, p.Nombre, p.Precio })
                .ToListAsync();
            return productos;
        }

        // PATCH: api/productos/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            foreach (var kv in updates)
            {
                if (kv.Key.ToLower() == "nombre") producto.Nombre = kv.Value?.ToString() ?? producto.Nombre;
                if (kv.Key.ToLower() == "precio" && decimal.TryParse(kv.Value?.ToString(), out var precio)) producto.Precio = precio;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/productos/borrarPorFiltro?minPrecio=10&maxPrecio=100
        [HttpDelete("borrarPorFiltro")]
        public async Task<IActionResult> DeleteByFilter(decimal? minPrecio = null, decimal? maxPrecio = null)
        {
            var query = _context.Productos.AsQueryable();
            if (minPrecio.HasValue) query = query.Where(p => p.Precio >= minPrecio);
            if (maxPrecio.HasValue) query = query.Where(p => p.Precio <= maxPrecio);
            var productos = await query.ToListAsync();
            if (!productos.Any()) return NotFound();
            _context.Productos.RemoveRange(productos);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Create(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
