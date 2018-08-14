using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBlog.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome recebe no máximo 100 Caracteres")]
        [DisplayName("Nome Completo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Usuario é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Login recebe no máximo 100 Caracteres")]
        [DisplayName("Usuário")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Email recebe no máximo 100 Caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Password { get; set; }

        [Required]
        [ScaffoldColumn(false)] // não criar a coluna via scaffolding
        [DisplayName("Horario Criado")]
        public DateTime Create_time { get; set; }

        [Required]
        [ScaffoldColumn(false)] // não criar a coluna via scaffolding
        [DisplayName("Horario Atualizado")]
        public DateTime Update_Time { get; set; }

        [Required]
        [DisplayName("Ultimo Acesso")]
        public DateTime Last_Login { get; set; }

        [Required(ErrorMessage = "O campo Ativo de Usuário é obrigatório")]
        [DisplayName("Usuario Ativo")]
        public bool ActiveUser { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório")]
        public UserType Types { get; set; }
        
        public virtual ICollection<Post> Posts { get; set; }
    }
}