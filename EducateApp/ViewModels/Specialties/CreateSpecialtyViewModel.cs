using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.ViewModels.Specialties
{
    public class CreateSpecialtyViewModel
    {
        [Required(ErrorMessage = "Введите индекс специальности")]
        [Display(Name = "Индекс")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Введите название специальности")]
        [Display(Name = "Специальность")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Формы обучения")]
        public short IdFormOfStudy { get; set; }   // будем передавать ИД формы обучения
    }
}
