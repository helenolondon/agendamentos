using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agendamentos.Servicos.Listas
{
    public class CompromissosListaDTO: List<CompromissoDTO>
    {
        public void LoadFromCompromissos(List<Compromisso> compromissos)
        {
            Clear();
            CompromissoDTO novo;

            foreach(var comp in compromissos)
            {
                novo = new CompromissoDTO();

                novo.LoadFromCompromisso(comp);
                
                this.Add(novo);
            }
        }
        public List<Compromisso> ToCompromissos()
        {
            if(this.Count == 0)
            {
                return null;
            }

            var lista = new List<Compromisso>();

            foreach(var dto in this)
            {
                lista.Add(dto.ToCompromisso());
            }

            return lista;
        }
    }
}
