using Agendamentos.Servicos.DTO;
using Agendamentos.Servicos.Erros;
using Agendamentos.Servicos.Listas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers.Apis
{
    [Route("api/compromissos")]
    public class CompromissosController : ApibaseController
    {
        /// <summary>
        /// Lista todos os compromissos - chamado pelo calendar
        /// </summary>
        [HttpGet]
        public IActionResult ListarCompromissos([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int codProfissional)
        {
            var servicos = this.CriarServicos();

            return Ok(servicos.CompromissosServico.ObterCompromissosPorProfissional(codProfissional, start, end, true));
        }

        [Route("profissional")]
        [HttpGet]
        public ActionResult<CompromissosListaDTO> ObterPorProfissional([FromQuery] int codProfissional, [FromQuery] DateTime data)
        {
            var serv = CriarServicos();

            var comps = serv.CompromissosServico.ObterCompromissosPorProfissional(codProfissional, data);

            if(comps == null || comps.Count == 0)
            {
                return Ok(new { data = new AgendamentoItensListaDTO() });
            }

            return Ok(new { data = comps });
        }

        [Route("profissional")]
        [HttpPost]
        public ActionResult<int> Salvar(CompromissoDTO compromisso)
        {
            var serv = CriarServicos();

            try
            {
                var codProfissional = serv.CompromissosServico.Salvar(compromisso);

                if (codProfissional == 0)
                {
                    return BadRequest("Não foi possível salvar o compromisso");
                }

                return Ok(codProfissional);
            }
            catch (ServicosException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("profissional")]
        [HttpDelete]
        public IActionResult Excluir([FromQuery] int codCompromisso)
        {
            var serv = CriarServicos();

            serv.CompromissosServico.Excluir(codCompromisso);

            return Ok();
        }
    }
}
