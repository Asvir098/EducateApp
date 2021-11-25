using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducateApp.Models.Data
{
    public class Discipline
    {
        // Key - поле первичный ключ
        // DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкременое
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название дисциплины")]
        [Display(Name = "Дисциплина")]
        public string Discip { get; set; }

        // так как у каждого пользователя (преподавателя) свой список дисциплин, то нужно указывать внешний ключ
        [Required]
        public string IdUser { get; set; }

        // Навигационные свойства
        // свойство нужно для белее правильного отображения данных в представлении
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
