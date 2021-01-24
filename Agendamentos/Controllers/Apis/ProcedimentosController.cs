using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
