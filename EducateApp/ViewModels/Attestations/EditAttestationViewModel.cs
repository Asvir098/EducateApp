using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Attestations
{
    public class EditAttestationViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "Аттестация")]
        public string Attest { get; set; }

        public string IdUser { get; set; }
    }
}
