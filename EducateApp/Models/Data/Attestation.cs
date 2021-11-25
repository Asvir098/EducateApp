using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducateApp.Models.Data
{
    public class Attestation
    {
        // Key - поле первичный ключ
        // DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкременое
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "Аттестация")]
        public string Attest { get; set; }

        // так как у каждого пользователя (преподавателя) свой список аттестаций, то нужно указывать внешний ключ
        [Required]
        public string IdUser { get; set; }

        // Навигационные свойства
        // свойство нужно для белее правильного отображения данных в представлении
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
