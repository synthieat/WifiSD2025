using SD.Resources;
using SD.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    public enum Ratings : byte
    {
        [LocalizedDescription(nameof(BasicRes.Ratings_0))]
        Unrated = 0,
        [LocalizedDescription(nameof(BasicRes.Ratings_10))]
        Bad = 10,
        [LocalizedDescription(nameof(BasicRes.Ratings_20))]
        Medium = 20,
        [LocalizedDescription(nameof(BasicRes.Ratings_30))]
        Great = 30
    }

    public abstract class MovieBase
    {        

        [Key] /* Nicht mehr notwendig by GUID */
        public virtual Guid Id { get; set; }

        [MaxLength(128), MinLength(2)]
        [Required]
        public virtual string Title { get; set; }   

        public virtual int GenreId { get; set; }
        public virtual string? MediumTypeCode { get; set; }

        public virtual decimal Price { get; set; }
        public virtual DateTime ReleaseDate { get; set; }

        public virtual Ratings Rating { get; set; } = 0;
    }
}
