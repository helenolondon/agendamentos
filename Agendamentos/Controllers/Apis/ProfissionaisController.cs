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
    public class ProfissionaisController : ApibaseController
    {
        [HttpGet]
        public IActionResult ListarProfissionais()
        {
            var servicos = this.CriarServicos();

            return Ok(servicos.PessoaServico.ListarProfissionais());
        }
    }
}
