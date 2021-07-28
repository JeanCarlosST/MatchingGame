using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int UsuarioId { get; set; }
        public int RatingValue { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
