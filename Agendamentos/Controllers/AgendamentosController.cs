using Agendamentos.Servicos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers
{
    public class AgendamentosController : Controller
    {
        // Agendamentos
        public ActionResult Index([FromQuery] string auth, [FromQuery] int cod_empresa, [FromQuery] int cod_usu)
        {
            var serv = new Agendamentos.ServicosNM.Servicos(Startup.Configuration);

            if (!(serv.AutenticaService.TokenValido(auth, cod_usu)))
            {
                return View("NaoAutorizado");
            }

            InfoUsuarioLoginDTO uInfo = serv.AutenticaService.ObterInfoUsuario(cod_usu);
            
            if(uInfo == null)
            {
                return View("NaoAutorizado");
            }

            ViewBag.Administrador = uInfo.Administrador;
            ViewBag.Profissional = uInfo.Funcionario;
            ViewBag.CodProfissional = cod_usu;
            ViewBag.NomeProfissional = uInfo.NomeUsuario;

            return View();
        }

        // Compromissos
        public PartialViewResult Compromissos()
        {
            return PartialView();
        }
        
        // Compromissos
        public PartialViewResult CompromissoForm()
        {
            return PartialView();
        }

        // Página de configurações da Agenda
        public ActionResult Configuracoes()
        {
            return View();
        }
    }
}
