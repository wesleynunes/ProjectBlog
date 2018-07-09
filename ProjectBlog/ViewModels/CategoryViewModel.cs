using ProjectBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBlog.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "O campo categoria é Obrigátorio")]
        [MaxLength(50, ErrorMessage = "O campo Nome recebe no máximo 50 Caracteres")]
        [DisplayName("Categoria")]
        [Index("Category_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [DisplayName("Horario Criado")]
        public DateTime Create_time { get; set; }

        [DisplayName("Horario Atualizado")]
        public DateTime Update_Time { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}