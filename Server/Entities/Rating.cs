using System;
using System.Collections.Generic;

#nullable disable

namespace MatchingGame.Server.Entities
{
    public partial class Rating
    {
        public int Rating1 { get; set; }
        public int UsuarioId { get; set; }
        public int RatingValue { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
