using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agendamentos.ServicosNM;


namespace Agendamentos.Controllers
{
    [ApiController]
    public class ApibaseController : ControllerBase
    {
        protected Agendamentos.ServicosNM.Servicos CriarServicos()
        {
            return new Agendamentos.ServicosNM.Servicos(Startup.Configuration);
        }
    }
}
