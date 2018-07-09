using ProjectBlog.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace ProjectBlog.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "O titulo é obrigatório")]
        [DisplayName("Titulo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O Conteudo do post é obrigatório")]
        [AllowHtml]
        [DisplayName("Conteudo")]
        public string Content { get; set; }

        [DisplayName("Horario Criado")]
        public DateTime Create_time { get; set; }

        [DisplayName("Horario Atualizado")]
        public DateTime Update_time { get; set; }

        [DisplayName("Tags")]
        public string Tag { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [NotMapped]
        [DisplayName("Imagen")]
        public HttpPostedFileBase ImageFile { get; set; }

        [Required(ErrorMessage = "O campo categoria e obrigatório")]
        [DisplayName("Categoria")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "O campo Usuario obrigatório")]
        [DisplayName("Usuario")]
        public int UserId { get; set; }

        public virtual Category Categories { get; set; }

        public virtual User Users { get; set; }


        public string Name { get; set; }

    }
}