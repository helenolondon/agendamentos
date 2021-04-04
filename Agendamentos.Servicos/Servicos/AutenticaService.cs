using Agendamentos.Infra;
using Agendamentos.Infra.Modelos;
using Agendamentos.Servicos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agendamentos.Servicos.Servicos
{
    public class AutenticaService: ServicoBase, IAutenticaService
    {
        public AutenticaService(IRepositorios repositorio): base(repositorio) { }

        public InfoUsuarioLoginDTO ObterInfoUsuario(int codUsuario)
        {
            var model = new InfoUsuarioLoginDTO();

            var uInfo = this.repositorio
                .AutenticaRepositorio
                .ObterUsuario(codUsuario);

            if(uInfo == null)
            {
                return null;
            }

            model.Administrador = uInfo.Grupos.Where(g => g.Grupo.Nome.ToUpper() == "ADMINISTRADOR").Any() ||
                uInfo.Grupos.Where(g => g.Grupo.Nome.ToUpper() == "INTEGRO").Any();

            model.Funcionario = uInfo.Grupos.Where(g => g.Grupo.Nome.ToUpper() == "PROFISSIONAIS").Any();
            model.NomeUsuario = uInfo.Nome;
            model.CodPessoa = uInfo.Cod_Pessoa ?? -1;

            return model;
        }

        public bool TokenValido(string auth, int codUsuartio)
        {
            TokensAcesso t = this.repositorio.AutenticaRepositorio.ObterToken(auth);

            if( t == null || t.Usr_Ident != codUsuartio)
            {
                return false;
            }

            return true;
        }
    }
}
