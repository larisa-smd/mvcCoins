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
    public class ReverseDesignersController : Controller
    {
        private CoinsDataEntities db = new CoinsDataEntities();

        // GET: ReverseDesigners
        public ActionResult Index( string sortOrder)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            var designers = from s in db.ReverseDesigners select s;
            
            if (sortOrder == "desc")
            {
                designers = designers.OrderByDescending(s => s.Name);
            }
            else
            {
                designers = designers.OrderBy(s => s.Name);
            }
            

            return View(designers.ToList());
        }

        // GET: ReverseDesigners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReverseDesigner reverseDesigner = db.ReverseDesigners.Find(id);
            if (reverseDesigner == null)
            {
                return HttpNotFound();
            }
            return View(reverseDesigner);
        }

        // GET: ReverseDesigners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReverseDesigners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesignerId,Name")] ReverseDesigner reverseDesigner)
        {
            if (ModelState.IsValid)
            {
                db.ReverseDesigners.Add(reverseDesigner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reverseDesigner);
        }

        // GET: ReverseDesigners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReverseDesigner reverseDesigner = db.ReverseDesigners.Find(id);
            if (reverseDesigner == null)
            {
                return HttpNotFound();
            }
            return View(reverseDesigner);
        }

        // POST: ReverseDesigners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesignerId,Name")] ReverseDesigner reverseDesigner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reverseDesigner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reverseDesigner);
        }

        // GET: ReverseDesigners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReverseDesigner reverseDesigner = db.ReverseDesigners.Find(id);
            if (reverseDesigner == null)
            {
                return HttpNotFound();
            }
            db.ReverseDesigners.Remove(reverseDesigner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: ReverseDesigners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReverseDesigner reverseDesigner = db.ReverseDesigners.Find(id);
            db.ReverseDesigners.Remove(reverseDesigner);
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
