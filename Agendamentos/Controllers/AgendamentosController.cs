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
        public ActionResult Index()
        {
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
