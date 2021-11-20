using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EducateApp.Models
{
    public class User : IdentityUser
    {
        //дополнительные поля для каждого пользователя
        //для преподавателя могут понадобится данные о ФИО

        [Required(ErrorMessage = "Введите фамилию")] //сообщение о ошибке при валидации на стороне клиента
        [Display(Name = "Фамилия")] //отображение Фамилия вместо LastName
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }


        //навигационные свойства
    }
}
