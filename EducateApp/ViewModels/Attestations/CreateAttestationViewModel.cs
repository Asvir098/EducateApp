using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Attestations
{
    public class CreateAttestationViewModel
    {
        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "Аттестация")]
        public string Attest { get; set; }
    }
}
