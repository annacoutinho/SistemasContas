using System.ComponentModel.DataAnnotations;

namespace SistemasContas.Mvc.Models
{
    public class ContasDashboardModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public string DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public string DataFim { get; set; }
    }
}
