using DreamCMS.Models;
using MvcPaging;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DreamCMS.Controllers
{
    public class ActivityController : Controller
    {

        DBFrontEnd db = new DBFrontEnd();

        // GET: Activity
        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            List<Post> ListPost = db.Posts.Where(x=>x.IsDisable==false).OrderByDescending(x=>x.PostId).ToList();

            return View(ListPost.ToPagedList(currentPageIndex, DDefault.DefaultPageSize));
        }


        public ActionResult Detail(string titleid)
        {
            Post post = db.Posts.Where(x => x.TitleId == titleid && x.IsDisable == false).FirstOrDefault();
            if (post == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }
            return View(post);
        }

        // GET: News
        public ActionResult IndexNews(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            List<News> ListNews = db.Newss.Where(x => x.IsDisable == false).OrderByDescending(x => x.NewsId).ToList();

            return View(ListNews.ToPagedList(currentPageIndex, DDefault.DefaultPageSize));
        }


        public ActionResult DetailNews(string titleid)
        {
            News news = db.Newss.Where(x => x.TitleId == titleid && x.IsDisable == false).FirstOrDefault();
            if (news == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }
            return View(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult IndexVideo(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            List<Videos> ListVideos= db.Videoss.Where(x => x.IsDisable == false).OrderByDescending(x => x.NewsId).ToList();

            return View(ListVideos.ToPagedList(currentPageIndex, DDefault.DefaultPageSize));
        }
        public ActionResult DetailVideo(string titleid)
        {
            Videos Video = db.Videoss.Where(x => x.TitleId == titleid && x.IsDisable == false).FirstOrDefault();
            if (Video == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }
            return View(Video);
        }


    }
}