 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionPrueba.Models;
using System.Security.Claims;
using System.Data;
using System.Data.SqlClient;

namespace GestionPrueba.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GestionPContext _context;
        private readonly GestionPContext2 _contexto;

        public UsuariosController(GestionPContext context, GestionPContext2 contexto)
        {
            _context = context;
            _contexto = contexto;
        }


        public async Task<IActionResult> Index()
        {

            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    if (c.FindFirst(ClaimTypes.Name)?.Value == "Gerente")
                    {
                        List<Usuario> lista = new List<Usuario>();

                        using (SqlConnection con = new(_contexto.Conexion))
                        {
                            using (SqlCommand cmd = new("sp_usuarios", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    lista.Add(new Usuario()
                                    {
                                        Usuario1 = (int)dr["usuario"],
                                        Nombre = dr["nombre"].ToString(),
                                        ApellidoP = dr["apellidoP"].ToString(),
                                        ApellidoM = dr["apellidoM"].ToString(),
                                        Cargo = dr["cargo"].ToString(),
                                        Sexo = dr["sexo"].ToString(),
                                        Edad = Convert.ToInt32(dr["edad"].ToString()),
                                        Correo = dr["correo"].ToString(),
                                        Contraseña = dr["contraseña"].ToString(),
                                        Estatus = Convert.ToBoolean(dr["estatus"].ToString())
                                    });
                                }
                                con.Close();
                            }
                        }
                        return View(lista);
                    }

            }
            return RedirectToAction("Login", "Cuenta");
        }


        // GET: Usuarios
        public async Task<IActionResult> Index2()
        {

            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    if (c.FindFirst(ClaimTypes.Name)?.Value == "Gerente")
                        return _context.Usuarios != null ?
                              View(await _context.Usuarios.ToListAsync()) :
                              Problem("Entity set 'GestionPContext.Usuarios'  is null.");

            }
            return RedirectToAction("Login", "Cuenta");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Usuario1 == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario1,Nombre,ApellidoP,ApellidoM,Cargo,Sexo,Edad,Correo,Contraseña,Estatus")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Usuario no registrado";
                
            }
            
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Usuario1,Nombre,ApellidoP,ApellidoM,Cargo,Sexo,Edad,Correo,Contraseña,Estatus")] Usuario usuario)
        {
            if (id != usuario.Usuario1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Usuario1))
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
                TempData["error"] = "Modificaci&oacute;n no realizada";
            }
            return View(usuario);
        }


        //
        public async Task<IActionResult> EditActivar(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivar(int id, [Bind("Usuario1,Nombre,ApellidoP,ApellidoM,Cargo,Sexo,Edad,Correo,Contraseña,Estatus")] Usuario usuario)
        {
            if (id != usuario.Usuario1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Usuario1))
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
                TempData["error"] = "Modificaci&oacute;n no realizada";
            }
            return View(usuario);
        }
        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Usuario1 == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("Usuario1,Nombre,ApellidoP,ApellidoM,Cargo,Sexo,Edad,Correo,Contraseña,Estatus")] Usuario usuario)
        {
            if (id != usuario.Usuario1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Alerta"] = "Usuario eliminado";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Usuario1))
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
            
            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Usuario1 == id)).GetValueOrDefault();
        }
    }
}
