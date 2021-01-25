using Agendamentos.Controllers.Apis.Requests;
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
        /// <summary>
        /// Lista todos os agendamentos
        /// </summary>
        [HttpGet]
        public IActionResult ListarAgendamentos()
        {
            var servicos = new Agendamentos.Servicos.Servicos();

            return Ok(servicos.AgendamentosServico.Listar());
        }

        [HttpPost]
        [Route("salvar")]
        public IActionResult SalvarAgendamento([FromBody]SalvarAgendamentoRequest salvarAgendamentoRequest)
        {
            return NoContent();
        }
    }
}
