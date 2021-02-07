using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.Controllers
{
    [ApiController]
    public class ApibaseController : ControllerBase
    {
        protected Agendamentos.Servicos.Servicos CriarServicos()
        {
            return new Agendamentos.Servicos.Servicos(Startup.Configuration);
        }
    }
}
