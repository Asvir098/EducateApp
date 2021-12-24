using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Disciplines
{
    public class CreateDisciplineViewModel
    {
        [Display(Name = "Индекс профессионального модуля")]
        public string IndexProfModule { get; set; }

        [Display(Name = "Название профессионального модуля")]
        public string ProfModule { get; set; }

        [Required(ErrorMessage = "Введите индекс")]
        [Display(Name = "Индекс")]
        public string Index { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите краткое название")]
        [Display(Name = "Краткое название")]
        public string ShortName { get; set; }
    }
}
