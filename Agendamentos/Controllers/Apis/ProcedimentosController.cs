using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agendamentos.Controllers.Apis.Requests;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Agendamentos.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedimentosController : ControllerBase
    {
        [HttpGet]
        [Route("{codPessoa:int}")]
        public IActionResult ListarPorPessoa(int codPessoa)
        {
            var servicos = new Agendamentos.Servicos.Servicos();

            return Ok(servicos.ProcedimentoServico.ListaPorPessoa(codPessoa));
        }

        [HttpGet]
        [Route("listar-profissionais-agendamento")]
        public IActionResult ListarPorfissionaisParaAgendamentos([FromQuery]ListarProfissionaisDisponiveisParaAgendamento request)
        {
            var servicos = new Agendamentos.Servicos.Servicos();
            var diaSemana = (int)request.Data.DayOfWeek + 1;

            var inicio = TimeSpan.Parse(request.HoraInicio);
            var termino = TimeSpan.Parse(request.HoraTermino);

            return Ok(servicos.ProcedimentoServico.ListarProfissionaisParaAgendamento(diaSemana, inicio, termino, request.CodServico));
        }
    }
}
