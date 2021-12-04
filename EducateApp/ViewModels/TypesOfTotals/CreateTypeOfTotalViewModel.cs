using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.TypesOfTotals
{
    public class CreateTypeOfTotalViewModel
    {
        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "Аттестация")]
        public string CertificateName { get; set; }
    }
}
