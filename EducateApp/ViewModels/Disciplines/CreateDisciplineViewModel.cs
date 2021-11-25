using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Disciplines
{
    public class CreateDisciplineViewModel
    {
        [Required(ErrorMessage = "Введите название дисциплины")]
        [Display(Name = "Дисциплина")]
        public string Discip { get; set; }
    }
}
