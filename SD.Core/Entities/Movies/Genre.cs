using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    [Table(name: nameof(Genre) + "s")]
    public class Genre : IEntity
    {
        public Genre() { 
            this.Movies = new HashSet<Movie>();
        }

        public virtual int Id { get; set; }

        [MaxLength(64)]
        [MinLength(2)]
        [Required]
        public virtual string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Movie> Movies {get;}
    }
}
