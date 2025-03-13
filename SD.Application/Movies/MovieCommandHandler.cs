using MediatR;
using Microsoft.EntityFrameworkCore;
using SD.Application.Base;
using SD.Core.Application.Commands;
using SD.Core.Application.Results;
using SD.Core.Attributes;
using SD.Core.Entities.Movies;
using SD.Core.Repositories.Movies;
using SD.Persistence.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieCommandHandler))]
    public class MovieCommandHandler : BaseHandler, IRequestHandler<DeleteMovieDtoCommand>,
                                                    IRequestHandler<CreateMovieDtoCommand, MovieDto>,
                                                    IRequestHandler<UpdateMovieDtoCommand, MovieDto>
    {
        private readonly IMovieRepository movieRepository;

        public MovieCommandHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task Handle(DeleteMovieDtoCommand command, CancellationToken cancellationToken)
        {
            await this.movieRepository.RemoveByKeyAsync<Movie>(command.Id, true, cancellationToken);
        }

        public async Task<MovieDto> Handle(CreateMovieDtoCommand command, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),    
                Title = "n/a",
                GenreId = 1,
                MediumTypeCode = "BR"                
            };

            /* Variante 1: Neues Movie wird in Db erstellt */
            await this.movieRepository.AddAsync(movie, true, cancellationToken);

            /* Gespeichertes 'Default'-Movie in DTO mappen und zurückgeben */
            return MovieDto.MapFrom(movie);
        }

        public async Task<MovieDto> Handle(UpdateMovieDtoCommand command, CancellationToken cancellationToken)
        {
            command.MovieDto.Id = command.Id;

            /* Nicht notwendig, da Movie mit der ID in der UpdateAsync Methode bereits auf 'Existierend' überprüft wird 
            var movie = await this.movieRepository.QueryFrom<Movie>(w => w.Id == command.Id).FirstOrDefaultAsync(cancellationToken);
            */

            //if(movie != null)
            //{
                var movie = new Movie();
                base.MapEntityProperties(command.MovieDto, movie);
                var updatedMovie = await this.movieRepository.UpdateAsync(movie, command.Id, true, cancellationToken);

                return MovieDto.MapFrom(updatedMovie);
            //}

            //return null;
        }
    }
}
