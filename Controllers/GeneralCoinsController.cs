using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using collectCoins.Models;
using System.IO;

namespace collectCoins.Controllers
{
    public class GeneralCoinsController : Controller
    {
        private CoinsDataEntities db = new CoinsDataEntities();

        // GET: GeneralCoins
        public ActionResult Index()
        {
            return View(db.GeneralCoins.ToList());
        }

        // GET: GeneralCoins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralCoin generalCoin = db.GeneralCoins.Find(id);
            if (generalCoin == null)
            {
                return HttpNotFound();
            }
            return View(generalCoin);
        }

        // GET: GeneralCoins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeneralCoins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GeneralCoinId,Value,Diameter,Weight,Thickness,Shape,Notes,Material")] GeneralCoin generalCoin, HttpPostedFileBase File)
        {
            if ((File != null) && (File.ContentLength > 0))
            {
                string fileName = generalCoin.Value.ToString() + "-pound.png";
                //string fileName = File.FileName;
                var path = Path.Combine(Server.MapPath("~/Content/Coins"),fileName);

                File.SaveAs(path);

            }

            if (ModelState.IsValid)
            {
                db.GeneralCoins.Add(generalCoin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(generalCoin);
        }

        // GET: GeneralCoins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralCoin generalCoin = db.GeneralCoins.Find(id);
            if (generalCoin == null)
            {
                return HttpNotFound();
            }

            ViewBag.Material = new SelectList(db.GeneralCoins.Select(m => m.Material).Distinct(), generalCoin.Material);
            ViewBag.Value = new SelectList(db.GeneralCoins.Select(m => m.Value).Distinct(),generalCoin.Value);
            ViewBag.Shape = new SelectList(db.GeneralCoins.Select(m => m.Shape).Distinct(), generalCoin.Shape);


            return View(generalCoin);
        }

        // POST: GeneralCoins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GeneralCoinId,Value,Diameter,Weight,Thickness,Shape,Notes,Material")] GeneralCoin generalCoin, string Image, HttpPostedFileBase File)
        {

            if ((File != null) && (File.ContentLength > 0))
            {
                string fileName = generalCoin.Value.ToString() + "-pound.png";
                //string fileName = File.FileName;
                var path = Path.Combine(Server.MapPath("~/Content/Coins"), fileName);

                File.SaveAs(path);
            }
            else
            {
                if (Image != null)
                {
                    if (!Image.Contains(generalCoin.Value.ToString()))
                    {
                        string imagePath = Server.MapPath("~/Content/Coins");
                        var oldFilePath = Path.Combine(imagePath, Image);
                        var newFilePath = Path.Combine(imagePath, generalCoin.Value.ToString() + "-pound.png");
                        if (!System.IO.File.Exists(newFilePath))
                            System.IO.File.Copy(oldFilePath, newFilePath);
                        else
                        {
                            System.IO.File.Create(newFilePath);
                        }
                    }
                }
                
            }

            if (ModelState.IsValid)
            {
                db.Entry(generalCoin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalCoin);
        }

        // GET: GeneralCoins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralCoin generalCoin = db.GeneralCoins.Find(id);
            if (generalCoin == null)
            {
                return HttpNotFound();
            }

            var coins = db.CoinsCollections.Where(m => m.GeneralCoinId == generalCoin.GeneralCoinId);

            if (coins.Count() <= 0)
            {
                db.GeneralCoins.Remove(generalCoin);
                db.SaveChanges();
            }
            else
            {
                foreach (var item in coins)
                {
                    CoinsCollection moneda = item;
                    db.CoinsCollections.Remove(moneda);
                   
                }

                db.GeneralCoins.Remove(generalCoin);
                db.SaveChanges();
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
