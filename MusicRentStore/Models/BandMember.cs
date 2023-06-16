using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class BandMember
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Imię")]
        public string Name { get; set;}
        [Display(Name ="Nazwisko")]
        public string Surname { get; set;}
        [Display(Name ="Instrument")]
        public string Instrument {get; set;}
        [Display(Name ="Data urodzin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set;}
        public ICollection<Artist> ?Artists { get; set; }
    }
}