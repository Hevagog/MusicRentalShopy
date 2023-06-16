using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set;}
        public User? User { get; set; }
        public Album? Album { get; set;}
    }
}