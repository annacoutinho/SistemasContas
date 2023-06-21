using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using SistemaContas.Domain.Entities;
using SistemaContas.Domain.Interfaces.Services;
using SistemaContas.Domain.Services;
using SistemasContas.Mvc.Models;


namespace SistemasContas.Mvc.Controllers
{

    public class ContasController : Controller
    {
        private readonly IContaDomainService _contaDomainService;

        public ContasController(IContaDomainService contaDomainService)
        {
            _contaDomainService = contaDomainService;
        }

        public IActionResult Dashboard()
        {
            var model = new ContasDashboardModel();

            try
            {
                model.DataInicio = new DateTime
                    (DateTime.Now.Year, DateTime.Now.Month, 1)
                    .ToString("yyyy-MM-dd");

                model.DataFim = new DateTime
                    (DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                    .ToString("yyyy-MM-dd");

                //consultando as contas:
                var contas = _contaDomainService.ConsultarContas
                    (DateTime.Parse(model.DataInicio), DateTime.Parse(model.DataFim));

                //gerando os dados para o gráfico
                TempData["TotalContasReceber"] = contas.Where(c => c.Tipo == TipoConta.Receber).Sum(c => c.Valor);
                TempData["TotalContasPagar"] = contas.Where(c => c.Tipo == TipoConta.Pagar).Sum(c => c.Valor);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }
       

        [HttpPost]
        public IActionResult Dashboard(ContasDashboardModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //consultando as contas:
                    var contas = _contaDomainService.ConsultarContas
                        (DateTime.Parse(model.DataInicio), DateTime.Parse(model.DataFim));

                    //gerando os dados para o gráfico
                    TempData["TotalContasReceber"] = contas.Where(c => c.Tipo == TipoConta.Receber).Sum(c => c.Valor);
                    TempData["TotalContasPagar"] = contas.Where(c => c.Tipo == TipoConta.Pagar).Sum(c => c.Valor);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Cadastro(ContasCadastroModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var conta = new Conta();
                    conta.IdConta = Guid.NewGuid();
                    conta.Nome = model.Nome;
                    conta.Data = DateTime.Parse(model.Data);
                    conta.Valor = decimal.Parse(model.Valor);

                    if (model.Tipo.Equals("Receber"))
                        conta.Tipo = TipoConta.Receber;
                    else if (model.Tipo.Equals("Pagar"))
                        conta.Tipo = TipoConta.Pagar;

                    _contaDomainService.CadastrarConta(conta);

                    TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', cadastrada com sucesso!";
                    ModelState.Clear();

                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consulta(ContasConsultaModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var dataMin = DateTime.Parse(model.DataInicio);
                    var dataMax = DateTime.Parse(model.DataFim);
                    var contas = _contaDomainService.ConsultarContas(dataMin, dataMax);
                    var nomeArquivo = $"contas {DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                    var tipoArquivo = string.Empty;
                    byte[] arquivo = null;

                    switch (model.Formato)

                    {
                        case "Excel":

                            nomeArquivo += ".xlsx";
                            tipoArquivo = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                            arquivo = _contaDomainService.GerarrelatorioExcel(contas);

                            break;

                        case "Pdf":
                            nomeArquivo += ".pdf";
                            tipoArquivo = "application/pdf";

                            arquivo = _contaDomainService.GerarRelatorioPdf(contas);
                            
                            return File(arquivo, tipoArquivo, nomeArquivo);

                        case "Html":
                            model.Contas = contas;

                            return View(model);
                    }
                    
                }
                catch(Exception e)
                {

                    TempData["MensagemErro"] = e.Message;

                }
            }
            return View();
        }
        public IActionResult Exclusao(Guid id)
        {
            try
            { 
              _contaDomainService.ExcluirConta(id);
                TempData["MensagemSucesso"] = "Conta excluída com sucesso!";

            }
            catch (Exception e)
            {
                TempData["MensagemErro"]= e.Message;
            }
            return RedirectToAction("Consulta");
        }
        public IActionResult Edicao(Guid id)
        {
            var model = new ContasEdicaoModel();

            try
            {
                var conta = _contaDomainService.ObterConta(id);

                model.IdConta = conta.IdConta;
                model.Nome = conta.Nome;
                model.Data = conta.Data.ToString("yyyy-MM-dd");
                model.Valor = conta.Valor.ToString();
                model.Tipo = conta.Tipo.ToString();
            
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(ContasEdicaoModel model)

        {

            if(ModelState.IsValid) 
            {
                try
                {
                    var conta = new Conta();

                    conta.IdConta = model.IdConta;
                    conta.Nome = model.Nome;
                    conta.Data =  Convert.ToDateTime(model.Data);
                    conta.Valor = decimal.Parse(model.Valor);


                    if (model.Tipo.Equals("Receber"))
                        conta.Tipo = TipoConta.Receber;
                    else if (model.Tipo.Equals("Pagar"))
                        conta.Tipo = TipoConta.Receber;

                    _contaDomainService.AtualizarConta(conta);

                    TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', atualizada com sucesso!";


                } 
                catch (Exception e)
                {
                    TempData["MensagemSucesso"] = "Conta excluída com sucesso!";
                }
            }
            return View(model);
        }
    }
}
