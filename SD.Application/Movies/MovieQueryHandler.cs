using MediatR;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SD.Core.Attributes;
using SD.Common.Extensions;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieQueryHandler))]
    public class MovieQueryHandler : IRequestHandler<GetMovieDtoQuery, MovieDto>,
                                     IRequestHandler<GetMovieDtosQuery, IEnumerable<MovieDto>>,
                                     IRequestHandler<GetGernesQuery, IEnumerable<Genre>>,
                                     IRequestHandler<GetMediumTypesQuery, IEnumerable<MediumType>>
    {
        protected readonly IMovieRepository movieRepository;
        public MovieQueryHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository; 
        }


        public async Task<MovieDto> Handle(GetMovieDtoQuery request, 
                                           CancellationToken cancellationToken)
        {
            var result = await this.GetMovieQueryWithNavigationPropertiesInitialized()
                                              .Where(w => w.Id == request.Id)
                                              .Select(s => MovieDto.MapFrom(s))
                                              .FirstOrDefaultAsync(cancellationToken);

            if(result != null)
            {
                result.LocalizedRating = result.Rating.GetDescription();
            }

            return result;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetMovieDtosQuery request, CancellationToken cancellationToken)
        {
            var movieQuery = this.GetMovieQueryWithNavigationPropertiesInitialized()
                                                 .Where(w => (!request.GenreId.HasValue || w.GenreId == request.GenreId) &&
                                                       (string.IsNullOrWhiteSpace(request.MediumTypeCode) || w.MediumTypeCode.Contains(request.MediumTypeCode)) &&
                                                       (string.IsNullOrWhiteSpace(request.SearchText) || w.Title.Contains(request.SearchText)))
                                                 .Take(request.Take) /* Pagination Take / Skip */
                                                 .Skip(request.Skip);

            var result = await movieQuery.Select(s => MovieDto.MapFrom(s))
                                         .ToListAsync(cancellationToken);

            result.ForEach(r =>
            {                
                r.LocalizedRating = r.Rating.GetDescription();
            });

            //var LocalizedRatingEnum = EnumExtension.EnumToList<Ratings>();

            return result;
        }

        public async Task<IEnumerable<Genre>> Handle(GetGernesQuery request, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<Genre>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MediumType>> Handle(GetMediumTypesQuery request, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<MediumType>().ToListAsync(cancellationToken);
        }

        private IQueryable<Movie> GetMovieQueryWithNavigationPropertiesInitialized()
        {
            return this.movieRepository.QueryFrom<Movie>().Include(i => i.Genre).Include(i => i.MediumType);
        }
    }
}
