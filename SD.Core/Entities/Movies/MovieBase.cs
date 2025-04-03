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

        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(BasicRes)), 
         MinLength(2, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(BasicRes))]
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(BasicRes))]
        [Display(Name = nameof(MovieBase.Title), ResourceType = typeof(BasicRes))]
        public virtual string Title { get; set; }

        [Display(Name = "Genre", ResourceType = typeof(BasicRes))]
        public virtual int GenreId { get; set; }

        [Display(Name = "MediumType", ResourceType = typeof(BasicRes))]
        public virtual string? MediumTypeCode { get; set; }

        [Display(Name = nameof(MovieBase.Price), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public virtual decimal Price { get; set; }

        [Display(Name = nameof(MovieBase.ReleaseDate), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime ReleaseDate { get; set; }
        
        [Display(Name = "Ratings", ResourceType = typeof(BasicRes))]
        public virtual Ratings Rating { get; set; } = 0;
    }
}
