using Microsoft.EntityFrameworkCore;
using MatchingGame.Server.Entities;

namespace MatchingGame.Server.DAL
{
    public class Context : DbContext
    {
        public Context() {  }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Amigo> Amigos { get; set; }
        public virtual DbSet<Dificultad> Dificultades { get; set; }
        public virtual DbSet<Modo> Modos { get; set; }
        public virtual DbSet<PartidaJugador> PartidaJugadores { get; set; }
        public virtual DbSet<Partida> Partida { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Tipo> Tipos { get; set; }
        public virtual DbSet<Torneo> Torneos { get; set; }
        public virtual DbSet<TorneoJugador> TorneoJugadores { get; set; }
        public virtual DbSet<TorneoPartida> TorneoPartida { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Properties.Resources.LocalCN);
        }
    }
}
