using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Disciplines
{
    public class EditDisciplineViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название дисциплины")]
        [Display(Name = "Дисциплина")]
        public string Discip { get; set; }

        public string IdUser { get; set; }
    }
}
