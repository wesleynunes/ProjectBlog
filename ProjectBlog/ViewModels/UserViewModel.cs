using ProjectBlog.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectBlog.ViewModels
{
    public class UserViewModel
    {
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
        
        [Required(ErrorMessage = "O campo Confrimar Senha é obrigatório")]
        [DataType(DataType.Password)]        
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [Compare(nameof(Password), ErrorMessage = "A senha e a confirmação não estão iguais")]
        [Display(Name = "Confirmar senha")]
        public string PasswordConfirmation { get; set; }
        
        [Required(ErrorMessage = "O campo Ativo de Usuário é obrigatório")]
        [DisplayName("Usuario Ativo")]
        public bool ActiveUser { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório")]
        public UserType Types { get; set; }
    }
}