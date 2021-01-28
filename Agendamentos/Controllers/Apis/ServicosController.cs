using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            var servicos = new Servicos.Servicos();

            var q = servicos.ServicosServico.Listar();

            if(q != null)
            {
                return Ok(q);
            }

            return NotFound();
        }
    }
}
