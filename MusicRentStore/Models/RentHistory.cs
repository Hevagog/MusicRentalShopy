using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class RentHistory
    {
        [Key]
        public int Id { get; set;}
        [Display(Name ="Nazwa Albumu")]
        public string AlbumTitle { get; set; }
        [Display(Name ="Data wypożyczenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  
        public DateTime DateOfRent { get; set;}
        [Display(Name ="Data oddania")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfReturn { get; set;}
        public ICollection<User> ?Users { get; set;}
    }
}