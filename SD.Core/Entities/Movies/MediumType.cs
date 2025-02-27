using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    public class MediumType : IEntity
    {
        public MediumType() 
        { 
            this.Movies = new HashSet<Movie>();        
        } 

        [MaxLength(8), MinLength(2)]
        [Key]
        public virtual string Code { get; set; }
        
        [MaxLength(32)]
        [MinLength(2)]
        [Required]
        public virtual string Name { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Movie> Movies { get; }

    }
}
