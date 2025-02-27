using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Entities.Movies
{
    public class Movie : MovieBase, IEntity
    {
        public Genre Genre { get; set; }

        [ForeignKey(nameof(MediumTypeCode))]
        public MediumType MediumType {get; set;}
    }
}
