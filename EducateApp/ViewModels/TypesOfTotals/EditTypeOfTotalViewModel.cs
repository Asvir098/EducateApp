using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.TypesOfTotals
{
    public class EditTypeOfTotalViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "Аттестация")]
        public string CertificateName { get; set; }

        public string IdUser { get; set; }
    }
}
