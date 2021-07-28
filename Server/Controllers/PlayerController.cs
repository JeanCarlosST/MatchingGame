using MatchingGame.Server.DAL;
using MatchingGame.Server.Services;
using MatchingGame.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly Context context;
        private readonly PlayerService playerService;

        public PlayerController(Context contexto)
        {
            context = contexto;
        }

        public void GetRecordRanking(Modo modo, Dificultad dificultad)
        {

        }

        public void GetUserRanking()
        {

        }

        [HttpPost("post_partida_solo")]
        public void PostPartidaSolo(Partida partida, string jwtToken)
        {
            playerService.GuardarPartidaSolo(partida, jwtToken);
        }

        [HttpPost("post_partida_1v1")]
        public void PostPartida1v1(Partida partida, string jwtToken)
        {
            playerService.GuardarPartida1v1(partida, jwtToken);
        }

        public void GetPlayerStats(string jwtToken)
        {

        }

        public void GetPlayerFriends(string jwtToken)
        {

        }

        public void GetPlayerRequests(string jwtToken)
        {

        }
    }
}
