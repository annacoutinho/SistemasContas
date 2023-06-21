using SistemaContas.Domain.Entities;
using SistemaContas.Domain.Interfaces.Reports;
using SistemaContas.Domain.Interfaces.Repositories;
using SistemaContas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Domain.Services
{
    public class ContaDomainService : IContaDomainService
    {
        private readonly IContaRepository _contaRepository;
        private readonly IContaReport _contaReport;
        public ContaDomainService(IContaRepository contaRepository, IContaReport contaReport)
        {
            _contaRepository = contaRepository;
            _contaReport = contaReport;
        }

        public void CadastrarConta(Conta conta)
        {
            #region REGRA: Não podemos cadastrar contas com data retroativa

            var dataAtual = DateTime.Now;
            if(conta.Data < new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day, 0, 0, 0))
            {
                throw new Exception(" O sistema não pode cadastrar contas com data retroativa.");
            }
            #endregion


            _contaRepository.Create(conta);
        }

        public void AtualizarConta(Conta conta)
        {
            #region REGRA: Não podemos atualizar conta que não exista no banco de dados

            if(_contaRepository.GetByID(conta.IdConta) == null)

            {
                throw new Exception(" A conta não foi encontrada no sistema, verifique o ID.");
            }

            #endregion
            _contaRepository.Update(conta);
        }

        public void ExcluirConta(Guid idConta)
        {

            #region REGRA: Não podemos excluir conta que não exista no banco de dados


            var conta = _contaRepository.GetByID(idConta);

            if (conta == null)

            {
                throw new Exception(" A conta não foi encontrada no sistema, verifique o ID.");
            }

            #endregion
            _contaRepository.Delete(conta);

        }

        public List<Conta> ConsultarContas(DateTime dataMin, DateTime dataMax)
        {
            #region REGRA: A dataMin não pode ser maior que a dataMax

            if(dataMin >= dataMax)
            {
                throw new Exception("A data de início deve ser menor ou igual a data de fim.");
            }

            #endregion

            return _contaRepository.GetAll(dataMin, dataMax);
        }

        public Conta ObterConta(Guid idConta)
        {
            #region REGRA: Não podemos retornar conta que não exista no banco de dados


            var conta = _contaRepository.GetByID(idConta);

            if (conta == null)

            {
                throw new Exception(" A conta não foi encontrada no sistema, verifique o ID.");
            }

            #endregion
            return conta;

        }

        public byte[] GerarrelatorioExcel(List<Conta> contas)
        {
            #region REGRA: A lista de contas não pode estar vazia

            if(contas.Count == 0)
            {
                throw new Exception("Não há dados para geração do relatório excel.");
            }

            #endregion

            return _contaReport.CreateExcel(contas);
        }

        public byte[] GerarRelatorioPdf(List<Conta> contas)
        {
            #region REGRA: A lista de contas não pode estar vazia

            if (contas == null || contas.Count == 0)
            {
                throw new Exception("Não há dados para geração do relatório pdf.");
            }

            #endregion

            return _contaReport.CreatePdf(contas);
        }

    }
    }
