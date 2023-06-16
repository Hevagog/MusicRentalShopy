using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Nazwa zespołu")]
        public string Name { get; set; }
        [Display(Name ="Kraj pochodzenia")]
        public string Country { get; set; }
        [Display(Name ="Data założenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  
        public DateTime DateOfFormation { get; set; }
        [Display(Name ="Członek zespołu")]
        public BandMember? BandMember { get; set;}
        public ICollection<Album> ?Albums { get; set;}
    }
}