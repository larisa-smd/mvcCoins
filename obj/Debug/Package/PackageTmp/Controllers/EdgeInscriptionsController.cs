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
    public class EdgeInscriptionsController : Controller
    {
        private CoinsDataEntities db = new CoinsDataEntities();

        // GET: EdgeInscriptions
        public ActionResult Index(string sortOrder)
        {
            ViewBag.InscriptionSort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            var inscriptions = from s in db.EdgeInscriptions select s;

            if (sortOrder == "desc")
            {
                inscriptions = inscriptions.OrderByDescending(s => s.Inscription);
            }
            else
            {
                inscriptions = inscriptions.OrderBy(s => s.Inscription);
            }

            return View(inscriptions.ToList());
        }

        // GET: EdgeInscriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EdgeInscription edgeInscription = db.EdgeInscriptions.Find(id);
            if (edgeInscription == null)
            {
                return HttpNotFound();
            }
            return View(edgeInscription);
        }

        // GET: EdgeInscriptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EdgeInscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EdgeId,Inscription,Translation")] EdgeInscription edgeInscription)
        {
            if (ModelState.IsValid)
            {
                db.EdgeInscriptions.Add(edgeInscription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(edgeInscription);
        }

        // GET: EdgeInscriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EdgeInscription edgeInscription = db.EdgeInscriptions.Find(id);
            if (edgeInscription == null)
            {
                return HttpNotFound();
            }
            return View(edgeInscription);
        }

        // POST: EdgeInscriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EdgeId,Inscription,Translation")] EdgeInscription edgeInscription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edgeInscription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edgeInscription);
        }

        // GET: EdgeInscriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EdgeInscription edgeInscription = db.EdgeInscriptions.Find(id);
            if (edgeInscription == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.EdgeInscriptions.Remove(edgeInscription);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                ViewBag.Message = "The inscription: " + edgeInscription.Inscription + " can not be deleted just yet! so we have this error: "+ e.Message;
            }
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
