using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FFCJardinagens.Models;

namespace FFCJardinagens.Controllers
{
    public class CartsController : Controller
    {
        private FFCJardinagensContext db = new FFCJardinagensContext();

        // GET: Carts
        public ActionResult Index()
        {
            return View(db.Carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClienteID,OrcamentoID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClienteID,OrcamentoID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


 

        public IList<Orcamento> getAllItems()
        {
            IList<Orcamento> orcamentos = new List<Orcamento>();
            orcamentos = db.Orcamentoes.ToList();
            return orcamentos;
        }

        public int AddCart(int orcamentoId, int ClienteID)
        {

            Cart objcart = new Cart()
            {
                OrcamentoID = orcamentoId,
                ClienteID = ClienteID
            };

            db.Carts.Add(objcart);
            db.SaveChanges();
            int count = db.Carts.Where(s => s.ClienteID == ClienteID).Count();
            return count;
        }


        public PartialViewResult GetCartItems(int ClienteID)
        {
  
            decimal sum = 0;
            var GetItemsId = db.Carts.Where(s => s.ClienteID == ClienteID).Select(u => u.OrcamentoID).ToList();
            var GetCartItem = from itemList in db.Orcamentoes where GetItemsId.Contains(itemList.ClienteID) select itemList;
            foreach (var totalsum in GetCartItem)
            {
                sum = sum + totalsum.ProdutoTotal;
            }
            ViewBag.Total = sum;
            return PartialView("_cartItem", GetCartItem);

        }

        public PartialViewResult DeleteCart(int clienteID)
        {
            Cart removeCart = db.Carts.FirstOrDefault(s => s.ClienteID == clienteID);
            db.Carts.Remove(removeCart);
            db.SaveChanges();

            return GetCartItems(clienteID);
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
