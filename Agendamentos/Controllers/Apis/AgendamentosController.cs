using Agendamentos.Controllers.Apis.Requests;
using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Erros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentosController : ApibaseController
    {
        /// <summary>
        /// Lista todos os agendamentos
        /// </summary>
        [HttpGet]
        public IActionResult ListarAgendamentos([FromQuery]DateTime start, [FromQuery] DateTime end, [FromQuery]int codProfissional)
        {
            var servicos = this.CriarServicos();

            return Ok(servicos.AgendamentosServico.Listar(start, end, codProfissional));
        }

        [HttpGet]
        [Route("{codAgendamento:int}")]
        public IActionResult ConsultaAgendamento(int codAgendamento)
        {
            var servicos = this.CriarServicos();
            var agendamento = servicos.AgendamentosServico.Consultar(codAgendamento);

            if(agendamento == null)
            {
                return NotFound();
            }

            return Ok(agendamento);
        }

        [HttpGet]
        [Route("configuracoes")]
        public IActionResult ObterConfiguracoes()
        {
            var serv = this.CriarServicos();

            var ret = serv.AgendamentosServico.ObterConfiguracoes();

            if(ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }

        [HttpPost]
        [Route("configuracoes")]
        public IActionResult SalvarConfiguracoes(AgendamentosConfiguracoesDTO request)
        {
            var serv = this.CriarServicos();

            if (serv.AgendamentosServico.SalvarConfiguracoes(request))
            {
                return Ok();
            }

            return BadRequest("Não foi possível salvar as configurações");
        }

        /// <summary>
        /// Salva um agendamento completo
        /// </summary>
        [HttpPost]
        [Route("Salvar")]
        public IActionResult SalvarAgendamento(AgendamentoDTO agendamento)
        {
            var servicos = this.CriarServicos();

            try
            {
                return Ok(servicos.AgendamentosServico.SalvarAgendamento(agendamento));
            }
            catch (ServicosException ex)
            {
                var erros = new ModelStateDictionary();
                erros.AddModelError("ERR", ex.Message);

                return BadRequest(erros);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Salva um iten de agendamento
        /// </summary>
        [HttpPost]
        [Route("salvar-item")]
        public IActionResult SalvarAgendamentoItem([FromBody]SalvarAgendamentoItemRequest salvarAgendamentoRequest)
        {
            var servicos = this.CriarServicos();

            var model = new AgendamentoItemDTO();

            model.CodAgendamento = salvarAgendamentoRequest.CodAgendamentoItem;
            model.CodCliente = 0;
            model.CodServico = salvarAgendamentoRequest.CodAgendamentoItem;
            model.CodProfissional = 0;
            model.Inicio = salvarAgendamentoRequest.HoraInicio;
            model.Termino = salvarAgendamentoRequest.HoraTermino;

            if(servicos.AgendamentosServico.SalvarAgendamentoItem(model) > 0)
            {
                return NoContent();
            }

            return BadRequest("Não foi possível salvar o agendamento");
        }

        /// <summary>
        /// Remove um agendamento
        /// </summary>
        [HttpDelete]
        [Route("remover")]
        public IActionResult Remover(int codAgendamentoItem)
        {
            var servicos = this.CriarServicos();
            if (servicos.AgendamentosServico.RemoverAgendamento(codAgendamentoItem))
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
