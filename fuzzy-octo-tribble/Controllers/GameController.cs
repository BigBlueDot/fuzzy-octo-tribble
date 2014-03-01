using fuzzy_octo_tribble.Models;
using GameDataClasses;
using PlayerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace fuzzy_octo_tribble.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public ActionResult Index()
        {
            checkGame();
            return View();
        }

        [HttpGet]
        public JsonResult GetMap()
        {
            Game game = checkGame();

            return Json(game.getClientRootMap(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlayer()
        {
            Game game = checkGame();

            return Json(game.getClientPlayer(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInteraction(int x, int y)
        {
            Game game = checkGame();

            return Json(game.getInteraction(x, y), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void OptionInteract(int x, int y, string option)
        {
            Game game = checkGame();

            game.setOptionInteraction(x, y, option);
        }

        [HttpGet]
        public void SetDungeon(int x, int y, string dungeonName, string party)
        {
            Game game = checkGame();

            string[] partyNames = party.Split(',');
            game.loadDungeon(x, y, dungeonName, partyNames);
        }

        [HttpGet]
        public void MoveLeft()
        {
            Game game = checkGame();
            game.moveLeft();
        }

        [HttpGet]
        public void MoveUp()
        {
            Game game = checkGame();
            game.moveUp();
        }

        [HttpGet]
        public void MoveRight()
        {
            Game game = checkGame();
            game.moveRight();
        }

        [HttpGet]
        public void MoveDown()
        {
            Game game = checkGame();
            game.moveDown();
        }

        private Game checkGame()
        {
            if (Session["Game"] == null) //Init game if not already
            {
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.Include(up => up.player)
                        .Include(up => up.player.characters)
                        .Include(up => up.player.characters.Select(c => c.characterClasses))
                        .Include(up => up.player.characters.Select(c => c.stats))
                        .FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
                    Session["Game"] = new GameDataClasses.Game(user.player);
                }
            }

            return ((Game)Session["Game"]);
        }
    }
}
