using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD.Application.Services;
using SD.Common.Extensions;
using SD.Core.Application.Commands;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;
using SD.Core.Entities.Movies;
using SD.Persistence.Repositories.DBContext;
using SD.Web.Extensions;

namespace SD.Web.Controllers
{
    [Authorize]
    public class MoviesController : MediatRBaseController
    {
        // private readonly MovieDbContext _context;

        //public MoviesController(MovieDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IApplicationCacheService applicationCacheService;

        public MoviesController(IApplicationCacheService applicationCacheService)
        {
            this.applicationCacheService = applicationCacheService;
        }
            

        // GET: Movies
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery]GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            var movieDtos = await base.Mediator.Send(query, cancellationToken);
            return View(movieDtos);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(GetMovieDtoQuery query, CancellationToken cancellationToken)
        {
            var movieDto = await base.Mediator.Send(query, cancellationToken);
            return View(movieDto);
        }

        /* Nicht notwendig, weil mit POST immer eine neue Movie Entität angelegt wird
        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            //ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
            ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code");
            return View();
        }
        */

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var movieDto = await base.Mediator.Send(new CreateMovieDtoCommand(), cancellationToken);
            movieDto.Title = string.Empty;
            await this.InitMovieDtoNavigationPropertiesFromCache(movieDto.GenreId, movieDto.MediumTypeCode, movieDto.Rating, cancellationToken);
            
            return View(movieDto);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = new GetMovieDtoQuery { Id = id.Value };
            var result = await base.Mediator.Send(query, cancellationToken);

            if(result == null)
            {
                return NotFound();
            }

            await this.InitMovieDtoNavigationPropertiesFromCache(result.GenreId, result.MediumTypeCode, result.Rating, cancellationToken);

            return View(result);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,GenreId,MediumTypeCode,Price,ReleaseDate,Rating")] Movie movie)
           Bind bei DTO nicht notwendig, da nur Properties enthalten sind, die man auch tatsächlich ändern kann 
         */
        public async Task<IActionResult> Edit([FromRoute]Guid id, MovieDto movieDto, CancellationToken cancellationToken)
        {
            if (id != movieDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var command = new UpdateMovieDtoCommand { Id = id, MovieDto = movieDto };
                    await base.Mediator.Send(command, cancellationToken);
                }
                catch 
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await this.InitMovieDtoNavigationPropertiesFromCache(movieDto.GenreId, movieDto.MediumTypeCode, movieDto.Rating, cancellationToken);
            return View(movieDto);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = new GetMovieDtoQuery{ Id = id.Value };

            var movieDto = await base.Mediator.Send(query, cancellationToken);

            if (movieDto == null)
            {
                return NotFound();
            }

            return View(movieDto);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieDtoCommand { Id = id };
            await base.Mediator.Send(command, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //private bool MovieExists(Guid id)
        //{
        //    return _context.Movies.Any(e => e.Id == id);
        //}

        private async Task InitMovieDtoNavigationProperties(int? genreId, string mediumTypeCode = default, Ratings? rating = default, 
                                                           CancellationToken cancellationToken = default)
        {
            var genres = HttpContext.Session.Get<IEnumerable<Genre>>(nameof(Genre));
            if (genres == null)
            {
                genres = await base.Mediator.Send(new GetGernesQuery(), cancellationToken);
                HttpContext.Session.Set(nameof(Genre), genres);
            }
            var genreSelectList = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name), genreId);

            var mediumTypes = HttpContext.Session.Get<IEnumerable<MediumType>>(nameof(MediumType));
            if (mediumTypes == null)
            {
                mediumTypes = await base.Mediator.Send(new GetMediumTypesQuery(), cancellationToken);
                HttpContext.Session.Set(nameof(MediumType), mediumTypes);
            }
            var mediumTypeSelectList = new SelectList(mediumTypes, nameof(MediumType.Code), nameof(MediumType.Name), mediumTypeCode);

            var localizedRatings = HttpContext.Session.Get<IEnumerable<KeyValuePair<object, string>>>(nameof(Ratings));
            if(localizedRatings == null)
            {
                localizedRatings = EnumExtension.EnumToList<Ratings>();
                HttpContext.Session.Set<IEnumerable<KeyValuePair<object, string>>>(nameof(Ratings), localizedRatings);
            }
            var localizedRatingsSelectList = new SelectList(localizedRatings, "Key", "Value", rating);

            ViewData[nameof(Genre)] = genreSelectList;
            ViewBag.MediumType = mediumTypeSelectList;
            ViewBag.Ratings = localizedRatingsSelectList;

        }

        private async Task InitMovieDtoNavigationPropertiesFromCache(int? genreId, string mediumTypeCode = default, Ratings? rating = default,
                                                                     CancellationToken cancellationToken = default)
        {
            var genres = await this.applicationCacheService.RetrieveFromCacheAsync(nameof(Genre),
                         () => base.Mediator.Send(new GetGernesQuery(), cancellationToken));
            var genreSelectList = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name), genreId);

            var mediumTypes = await this.applicationCacheService.RetrieveFromCacheAsync(nameof(MediumType),
                         () => base.Mediator.Send(new GetMediumTypesQuery(), cancellationToken));
            var mediumTypeSelectList = new SelectList(mediumTypes, nameof(MediumType.Code), nameof(MediumType.Name), mediumTypeCode);

            var localizedRatings = this.applicationCacheService.RetrieveFromCache("LocalizedRatings",
                         () => EnumExtension.EnumToList<Ratings>());
            var localizedRatingsSelectList = new SelectList(localizedRatings, "Key", "Value", rating);

            ViewData[nameof(Genre)] = genreSelectList;
            ViewBag.MediumType = mediumTypeSelectList;
            ViewBag.Ratings = localizedRatingsSelectList;

        }
    }
}
