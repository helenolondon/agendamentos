using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentosController : ControllerBase
    {
        public IActionResult ListarServicos()
        {
            //return Ok();
            
            var servicos = new Agendamentos.Servicos.Servicos();

            return Ok(servicos.AgendamentosServico.Listar());
        }
    }
}
