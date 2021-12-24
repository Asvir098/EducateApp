using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducateApp.Models.Data
{
    public class Discipline
    {
        // Key - поле первичный ключ
        // DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкременое
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Идентификатор")]
        public short Id { get; set; }

        
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

        // так как у каждого пользователя (преподавателя) свой список дисциплин, то нужно указывать внешний ключ
        [Required]
        public string IdUser { get; set; }

        // Навигационные свойства
        // свойство нужно для белее правильного отображения данных в представлении
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
