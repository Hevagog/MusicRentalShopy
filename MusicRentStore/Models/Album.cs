using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{

    public class Album
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Ilość płyt w magazynie")]
        public int capacity { get; set;}
        [Display(Name ="Nazwa albumu")]
        public string Name { get; set; }
        [Display(Name ="Artysta")]
        public Artist? Artist { get; set; }
        [Display(Name ="Data wydania")]
        public DateTime ReleaseDate { get; set;}
        [Display(Name ="Gatunek")]
        public string Genre { get; set; }
        public ICollection<Store> ?Stores {get; set;} 
    }

}