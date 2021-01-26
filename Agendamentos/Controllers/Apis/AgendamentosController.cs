using Agendamentos.Controllers.Apis.Requests;
using Agendamentos.Servicos.DTO;
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
            var servicos = new Agendamentos.Servicos.Servicos();

            var model = new AgendamentoDTO();

            model.CodAgendamento = salvarAgendamentoRequest.CodAgendamento ?? 0;
            model.CodCliente = "0";
            model.CodProcedimento = salvarAgendamentoRequest.CodProcedimento ?? 0;
            model.CodProfissional = 0;
            model.Inicio = DateTime.Parse(salvarAgendamentoRequest.HoraInicio);
            model.Termino = DateTime.Parse(salvarAgendamentoRequest.HoraTermino);

            if(servicos.AgendamentosServico.SalvarAgendamento(model) > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("remover")]
        public IActionResult Remover(int codAgendamento)
        {
            var servicos = new Agendamentos.Servicos.Servicos();
            if (servicos.AgendamentosServico.RemoverAgendamento(codAgendamento))
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
