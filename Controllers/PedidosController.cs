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
    public class PedidosController : Controller
    {
        private readonly GestionPContext _context;
        private readonly GestionPContext2 _contexto;

        public PedidosController(GestionPContext context, GestionPContext2 contexto)
        {
            _context = context;
            _contexto = contexto;
        }

     

        public async Task<IActionResult> Index3()
        {
            return _context.Productos != null ?
                        View(await _context.Productos.ToListAsync()) :
                        Problem("Entity set 'GestionPContext.Productos'  is null.");
        }
        public async Task<IActionResult> IndexCocinero()
        {
            return _context.Productos != null ?
                        View(await _context.Productos.ToListAsync()) :
                        Problem("Entity set 'GestionPContext.Productos'  is null.");
        }

        public async Task<IActionResult> IndexTodos()
        {
            var gestionPContext = _context.Pedidos.Include(p => p.IdProductoNavigation).Include(p => p.UsuarioNavigation);
            return View(await gestionPContext.ToListAsync());
        }
        public async Task<IActionResult> Index2()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    if (c.FindFirst(ClaimTypes.Name)?.Value == "Cocinero")
                    {
                        List<Pedido> lista = new List<Pedido>();

                        using (SqlConnection con = new(_contexto.Conexion))
                        {
                            using (SqlCommand cmd = new("sp_pedidos1", con))
                            {
                                try
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    con.Open();
                                    var dr = cmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        lista.Add(new Pedido()
                                        {
                                            IdPedido = (int)dr["idPedido"],
                                            IdProducto = (int)dr["idProducto"],
                                            Usuario = (int)dr["usuario"],
                                            Cantidad = (int)dr["Cantidad"],
                                            NumeroMesa = (int)dr["numeroMesa"],
                                            Nota = dr["Nota"].ToString(),
                                            Estatus = dr["estatus"].ToString()
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
        }


        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            //var gestionPContext = _context.Pedidos.Include(p => p.IdProductoNavigation).Include(p => p.UsuarioNavigation);
            //return View(await gestionPContext.ToListAsync());
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    if (c.FindFirst(ClaimTypes.Name)?.Value == "Mesero")
                    {
                        List<Pedido> lista = new List<Pedido>();

                        using (SqlConnection con = new(_contexto.Conexion))
                        {
                            using (SqlCommand cmd = new("sp_pedidos1", con))
                            {
                                try
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    con.Open();
                                    var dr = cmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        lista.Add(new Pedido()
                                        {
                                            IdPedido = (int)dr["idPedido"],
                                            IdProducto = (int)dr["idProducto"],
                                            Usuario = (int)dr["usuario"],
                                            Cantidad = (int)dr["Cantidad"],
                                            NumeroMesa = (int)dr["numeroMesa"],
                                            Nota = dr["Nota"].ToString(),
                                            Estatus = dr["estatus"].ToString()
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

        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.UsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,IdProducto,Usuario,Cantidad,NumeroMesa,Nota,Estatus")] Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                                _context.Add(pedido);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Pedido no guardado";

                } 
            }
            
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,IdProducto,Usuario,Cantidad,NumeroMesa,Nota,Estatus")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
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

            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }
        //Delete de pedidos por parte del mesero
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("IdPedido,IdProducto,Usuario,Cantidad,NumeroMesa,Nota,Estatus")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    TempData["Alerta"] = "Pedido no guardado";
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }
        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete1(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete1(int id, [Bind("IdPedido,IdProducto,Usuario,Cantidad,NumeroMesa,Nota,Estatus")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    TempData["Alerta"] = "Pedido no guardado";
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index2));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", pedido.IdProducto);
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Usuario1", "Usuario1", pedido.Usuario);
            return View(pedido);
        }

        private bool PedidoExists(int id)
        {
          return (_context.Pedidos?.Any(e => e.IdPedido == id)).GetValueOrDefault();
        }
    }
}
