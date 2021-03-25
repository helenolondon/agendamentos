using Agendamentos.Servicos.DTO;
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
        [Route("profissional")]
        [HttpGet]
        public ActionResult<CompromissosListaDTO> ObterPorProfissional([FromQuery] int codProfissional)
        {
            var serv = CriarServicos();

            var comps = serv.CompromissosServico.ObterCompromissosPorProfissional(codProfissional);

            if(comps == null || comps.Count == 0)
            {
                return NotFound();
            }

            return Ok(comps);
        }

        [Route("profissional")]
        [HttpPost]
        public ActionResult<int> Salvar(CompromissoDTO compromisso)
        {
            var serv = CriarServicos();

            var codProfissional = serv.CompromissosServico.Salvar(compromisso);

            if(codProfissional == 0)
            {
                return BadRequest("Não foi possível salvar o compromisso");
            }

            return Ok(codProfissional);
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
