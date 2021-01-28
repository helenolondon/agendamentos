using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class ServicosListaDTO : List<ServicoDTO>
    {
        public List<Servico> ToServicoLista()
        {
            var temp = new List<Servico>();

            this.ForEach((e) =>
            {
                temp.Add(new Servico()
                {
                    Id_Servico = e.CodServico,
                    Nome_Servico = e.NomeServico
                });
            });

            return temp;
        }
        public void LoadFromServicoLista(List<Servico> lista)
        {
            this.Clear();
            ServicoDTO temp;

            lista.ForEach((e) =>
            {
                temp = new ServicoDTO();
                temp.LoadFromServico(e);

                this.Add(temp);
            });
        }
    }
}
