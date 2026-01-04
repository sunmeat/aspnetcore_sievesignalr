using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Soccer.Models
{
    public class Player
    {
        [Display(Name = "Айді")]
        public int Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Display(Name = "Ім'я футболіста")]
        public string? Name { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Display(Name = "Вік")]
        public int Age { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Display(Name = "Позиція на полі")]
        public string? Position { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] // для фильтра по команді
        [Display(Name = "Айді команди")]
        public int TeamId { get; set; }

        public Team? Team { get; set; }
    }
}