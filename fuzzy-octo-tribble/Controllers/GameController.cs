using fuzzy_octo_tribble.Models;
using GameDataClasses;
using PlayerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CombatDataClasses.Interfaces;

namespace fuzzy_octo_tribble.Controllers
{
    [Authorize]
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
        public JsonResult MoveLeft()
        {
            Game game = checkGame();
            return Json(game.moveLeft(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MoveUp()
        {
            Game game = checkGame();
            return Json(game.moveUp(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MoveRight()
        {
            Game game = checkGame();
            return Json(game.moveRight(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MoveDown()
        {
            Game game = checkGame();
            return Json(game.moveDown(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult isInDungeon()
        {
            Game game = checkGame();
            return Json(new { isInDungeon = game.isInDungeon() }, JsonRequestBehavior.AllowGet);
        }

        private Game checkGame()
        {
            if (Session["Game"] == null) //Init game if not already
            {
                UsersContext db = new UsersContext();
                GameDataClasses.Game game = new Game(User.Identity.Name, db);
                Session["Game"] = game;
            }

            return ((Game)Session["Game"]);
        }

        [HttpGet]
        public JsonResult getCommands()
        {
            Game game = checkGame();
            return Json(game.getCommands(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getStatus()
        {
            Game game = checkGame();
            return Json(game.getStatus(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult nextTurn()
        {
            Game game = checkGame();
            return Json(game.nextTurn(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult executeCommand(SelectedCommand command)
        {
            Game game = checkGame();
            return Json(game.executeCommand(command));
        }

        [HttpGet]
        public void setClass(int x, int y, string characterName, string className)
        {
            Game game = checkGame();
            game.setClass(x, y, characterName, className);
        }

        [HttpGet]
        public JsonResult getMessages()
        {
            Game game = checkGame();
            return Json(game.getMessages(), JsonRequestBehavior.AllowGet);
        }
    }
}
