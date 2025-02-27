using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    public abstract class MovieBase
    {
        [Key] /* Nicht mehr notwendig by GUID */
        public virtual Guid Id { get; set; }

        [MaxLength(128), MinLength(2)]
        [Required]
        public virtual string Title { get; set; }   

        public int GenreId { get; set; }
        public string MediumTypeCode { get; set; }

        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
