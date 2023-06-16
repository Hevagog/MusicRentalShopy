using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class User
        {
            [Key]
            public int Id { get; set; }
            [Display(Name ="Nazwa Użytkownika")]
            public string UserName { get; set; }
            [Display(Name ="Historia")]
            public RentHistory? RentHistory { get; set;}
            public ICollection<Store> ?Stores {get; set;}
        }
}