using SistemaContas.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace SistemasContas.Mvc.Models
{
    public class ContasConsultaModel
    {
        [Required(ErrorMessage = "por favor, informe a data de início.")]
        public string DataInicio { get; set; }

        [Required(ErrorMessage = "por favor, informe a data de término.")]
        public string DataFim { get; set; }

        [Required(ErrorMessage = "por favor, informe o formato do relatório.")]
        public string Formato { get; set; }

        public List<Conta>? Contas { get; set; }

    }
}
