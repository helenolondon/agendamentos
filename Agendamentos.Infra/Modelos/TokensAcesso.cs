using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Agendamentos.Infra.Modelos
{
    [Table("ADM_AUTEN")]
    public class TokensAcesso
    {
        [Key]
        public int Id { get; set; }
        public int Usr_Ident { get; set; }
        public string Usr_Token { get; set; }
    }
}
