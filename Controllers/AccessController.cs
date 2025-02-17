using GestionPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Mail;

namespace Gestion.Controllers
{
    public class AccessController : Controller
    {

        private readonly GestionPContext2 _contexto;

        public AccessController(GestionPContext2 contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StartRecovery()
        {
            Usuario model = new Usuario();
            return View(model);
        }

        [HttpPost]
        public IActionResult StartRecovery(Usuario model)
        {

            using (SqlConnection con = new(_contexto.Conexion))
            {
                using (SqlCommand cmd = new("sp_recovery", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", System.Data.SqlDbType.VarChar).Value = model.Usuario1;
                    cmd.Parameters.Add("@correo", System.Data.SqlDbType.VarChar).Value = model.Correo;
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["usuario"] != null && model.Usuario1 != null)
                        {
                            SendEmail(model.Correo, dr["contraseña"].ToString());
                            return RedirectToAction("Login", "Cuenta");
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

        #region
        public void SendEmail(string email, string rContraseña)
        {
            MailMessage mailMessage = new MailMessage("ben.wingo.website@gmail.com", email, "Recuperar Contraseña",
                "<p>Contraseña</p><h3>" + rContraseña + "</h3>");
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("ben.wingo.website@gmail.com", "qrajegkqomoxzimp");

            smtpClient.Send(mailMessage);

            smtpClient.Dispose();
        }

        #endregion

    }
}
