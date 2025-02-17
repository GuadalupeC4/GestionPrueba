using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionPrueba.Models;
using System.Security.Claims;
using System.Data.SqlClient;
using System.Data;

namespace GestionPrueba.Controllers
{
    public class ProductosController : Controller
    {
        private readonly GestionPContext _context;
        private readonly GestionPContext2 _contexto;


        public ProductosController(GestionPContext context, GestionPContext2 contexto)
        {
            _context = context;
            _contexto = contexto;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    if (c.FindFirst(ClaimTypes.Name)?.Value == "Gerente")
                    {
                        List<Producto> lista = new List<Producto>();

                        using (SqlConnection con = new (_contexto.Conexion))
                        {
                            using (SqlCommand cmd = new("sp_producto", con))
                            {
                                try
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    con.Open();
                                    var dr = cmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        lista.Add(new Producto()
                                        {
                                            IdProducto = (int)dr["idProducto"],
                                            Nombre = dr["nombre"].ToString(),
                                            Precio = (decimal)dr["precio"],
                                            Ingredientes = dr["ingredientes"].ToString(),
                                            Categoria = dr["categoria"].ToString(),
                                            Aditivos = dr["aditivos"].ToString(),
                                            Estatus = Convert.ToBoolean(dr["estatus"].ToString())
                                        });
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                                con.Close();
                            }
                        }
                        return View(lista);
                    }

            }
            return RedirectToAction("Login", "Cuenta");
            //return _context.Productos != null ? 
                        //View(await _context.Productos.ToListAsync()) :
                        //Problem("Entity set 'GestionPContext.Productos'  is null.");
        }

        //Index Todos
        public async Task<IActionResult> IndexTodos()
        {
            
            return _context.Productos != null ? 
            View(await _context.Productos.ToListAsync()) :
            Problem("Entity set 'GestionPContext.Productos'  is null.");
        }



        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Nombre,Precio,Ingredientes,Categoria,Aditivos,Estatus")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Producto no registrado";
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Nombre,Precio,Ingredientes,Categoria,Aditivos,Estatus")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Modificacion no realizada";
            }
            return View(producto);
        }

        //Editar Activar
        public async Task<IActionResult> EditActivar(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivar(int id, [Bind("IdProducto,Nombre,Precio,Ingredientes,Categoria,Aditivos,Estatus")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Modificacion no realizada";
            }
            return View(producto);
        }


        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("IdProducto,Nombre,Precio,Ingredientes,Categoria,Aditivos,Estatus")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Alerta"] = "Modificacion no realizada";
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            
            return View(producto);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("IdProducto,Nombre,Precio,Ingredientes,Categoria,Aditivos,Estatus")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Producto no eliminado";
            }
            return View(producto);
        }*/

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }
    }
}
