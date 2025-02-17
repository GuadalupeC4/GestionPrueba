using GestionPrueba.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace GestionPrueba.Controllers
{
    public class CuentaController : Controller
    {
        private readonly GestionPContext2 _contexto;

        public CuentaController(GestionPContext2 contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                    
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("sp_validar", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usuario", System.Data.SqlDbType.VarChar).Value = u.Usuario1;
                        cmd.Parameters.Add("@contraseña", System.Data.SqlDbType.VarChar).Value = u.Contraseña;
                        con.Open();
                        var dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr["usuario"] != null && u.Usuario1 != null)
                            {
                                List<Claim> c = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, u.Contraseña),
                                    new Claim(ClaimTypes.Name, dr["cargo"].ToString())
                                };

                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();

                                p.AllowRefresh = true;
                                p.IsPersistent = u.MantenerActivo;

                                //Tiempor de expiración de sesión
                                if (!u.MantenerActivo)
                                {
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3);
                                }
                                else
                                {
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
                                }
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                if (dr["cargo"].ToString() == "Gerente")
                                    return RedirectToAction("Index", "Home");
                                else if (dr["cargo"].ToString() == "Mesero")
                                    return RedirectToAction("Index", "Pedidos");
                                else if (dr["cargo"].ToString() == "Cocinero")
                                    return RedirectToAction("IndexCocinero", "Pedidos");
                            }
                            else
                            {
                                
                                ViewBag.Error = "Credenciales incorrectas o cuenta no registrada";
                            }
                        }
                        con.Close();
                    }
                    return View();
                }
            }
            catch (System.Exception ex)
            {
                TempData["dat"] = "Usuario no eliminado";
                
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
