using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using collectCoins.Models;

namespace collectCoins.Controllers
{
    public class PortretDesignersController : Controller
    {
        private CoinsDataEntities db = new CoinsDataEntities();

        // GET: PortretDesigners
        public ActionResult Index( string sortOrder)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            var designers = from s in db.PortretDesigners select s;

            switch (sortOrder)
            {
                case "desc":
                    designers = designers.OrderByDescending(s => s.Name);
                    break;
                default:
                    designers = designers.OrderBy(s => s.Name);
                    break;
            }

            return View(designers.ToList());
        }

        // GET: PortretDesigners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortretDesigner portretDesigner = db.PortretDesigners.Find(id);
            if (portretDesigner == null)
            {
                return HttpNotFound();
            }
            return View(portretDesigner);
        }

        // GET: PortretDesigners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PortretDesigners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesignerId,Name")] PortretDesigner portretDesigner)
        {
            if (ModelState.IsValid)
            {
                db.PortretDesigners.Add(portretDesigner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portretDesigner);
        }

        // GET: PortretDesigners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortretDesigner portretDesigner = db.PortretDesigners.Find(id);
            if (portretDesigner == null)
            {
                return HttpNotFound();
            }
            return View(portretDesigner);
        }

        // POST: PortretDesigners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesignerId,Name")] PortretDesigner portretDesigner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portretDesigner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portretDesigner);
        }

        // GET: PortretDesigners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortretDesigner portretDesigner = db.PortretDesigners.Find(id);
            if (portretDesigner == null)
            {
                return HttpNotFound();
            }
            db.PortretDesigners.Remove(portretDesigner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
