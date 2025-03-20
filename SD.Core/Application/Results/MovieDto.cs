using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Application.Results
{
    public class MovieDto : MovieBase
    {
        public string GenreName { get; set; }
        public string MediumTypeName { get; set; }  

        public static MovieDto MapFrom(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                GenreId = movie.GenreId,            
                GenreName = movie.Genre != null ? movie.Genre.Name : string.Empty,    
                MediumTypeCode = movie.MediumTypeCode,
                MediumTypeName = movie.MediumType != null ? movie.MediumType.Name : string.Empty,
                Rating = movie.Rating,
                Price = movie.Price
            };
        }

    }
}
