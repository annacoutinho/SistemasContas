using SistemaContas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Domain.Interfaces.Services
{
    public interface IContaDomainService
    {
        #region Métodos abstratos

        void CadastrarConta(Conta conta);

        void AtualizarConta(Conta conta);
        void ExcluirConta(Guid idConta);
        List<Conta> ConsultarContas(DateTime DataMin, DateTime DataMax);
        Conta ObterConta(Guid idConta);


        byte[] GerarrelatorioExcel(List<Conta> contas);
        byte[] GerarRelatorioPdf(List<Conta> contas);

        #endregion


    }
}
