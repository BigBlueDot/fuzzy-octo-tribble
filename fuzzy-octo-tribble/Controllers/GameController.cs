using fuzzy_octo_tribble.Models;
using GameDataClasses;
using PlayerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            return Json(game.getClientRootMap());
        }

        private Game checkGame()
        {
            if (Session["Game"] == null) //Init game if not already
            {
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
                    Session["Game"] = new GameDataClasses.Game(user.player);
                }
            }

            return ((Game)Session["Game"]);
        }
    }
}
