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
        [Route("Salvar")]
        public IActionResult SalvarAgendamento(SalvarAgendamentoRequest agendamento)
        {
            return Ok(agendamento);
        }

        [HttpPost]
        [Route("salvar-item")]
        public IActionResult SalvarAgendamentoItem([FromBody]SalvarAgendamentoItemRequest salvarAgendamentoRequest)
        {
            var servicos = new Agendamentos.Servicos.Servicos();

            var model = new AgendamentoItemDTO();

            model.CodAgendamento = salvarAgendamentoRequest.CodAgendamentoItem;
            model.CodCliente = "0";
            model.CodServico = salvarAgendamentoRequest.CodAgendamentoItem;
            model.CodProfissional = 0;
            model.Inicio = salvarAgendamentoRequest.HoraInicio;
            model.Termino = salvarAgendamentoRequest.HoraTermino;

            if(servicos.AgendamentosServico.SalvarAgendamentoItem(model) > 0)
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
