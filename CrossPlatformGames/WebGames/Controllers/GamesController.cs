using Microsoft.AspNetCore.Mvc;
using GameWebApi.Data;
using System;
using Newtonsoft.Json;

namespace GameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private IGamesData _data;
        public GamesController(IGamesData data)
        {
            _data = data;
        }

        [HttpPost]
        public IActionResult CreateJoinGame(object obj)
        {
            var game = _data.CreateJoinGame(obj);
            if (game != null)
                return Ok(game);
            return NotFound();

            // return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + game.Id, game);
        }

        [HttpPut("{id}/{usr}")]
        public IActionResult UpdateGame(object obj, int id, int usr)
        {
            var data = JsonConvert.DeserializeObject<Tuple<string, int, int>>(obj.ToString());
            string name = data.Item1;
            int x = data.Item2;
            int y = data.Item3;
            var res = _data.UpdateGame(name, x, y, id, usr);
            if (res != null)
                return Ok(res);
            return NotFound();
        }

        [HttpPut]
        public IActionResult ResetGame(object obj, int id)
        {
            var name = JsonConvert.DeserializeObject<string>(obj.ToString());
            var res = _data.ResetGame(name,id);
            if (res != null)
                return Ok(res);
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            return Ok(_data.GetGames());
        }

        [HttpGet("{name}/{id}/{usr}")]
        public IActionResult GetGame(string name, int id, int usr)
        {
            if (id == -1)
                return NotFound();
           
            var game = _data.GetGameState(name, id, usr);
            //var game = _data.GetGame(id);

            if (game != null)
                return Ok(game);
            return NotFound();
        }

        [HttpDelete]
        public IActionResult DeleteGame(object obj)
        {
            var t = JsonConvert.DeserializeObject<Tuple<string, int>>(obj.ToString());

            if (_data.DeleteGame(t.Item1, t.Item2))
                return Ok();
            return NotFound();
        }
    }
}
