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
    public class TotalOrcamentoesController : Controller
    {
        private FFCJardinagensContext db = new FFCJardinagensContext();

        // GET: TotalOrcamentoes
        public ActionResult Index()
        {
            return View(db.TotalOrcamentoes.ToList());
        }

        // GET: TotalOrcamentoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalOrcamento totalOrcamento = db.TotalOrcamentoes.Find(id);
            if (totalOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(totalOrcamento);
        }

        // GET: TotalOrcamentoes/Create
        public ActionResult Create()
        {
            var clientes = db.Clientes.ToList();

            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            return View();
        }

        // POST: TotalOrcamentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TotalOrcamento totalOrcamento)
        {
            var clientes = db.Clientes.ToList();

            var cliente = db.Clientes.Find(totalOrcamento.ClienteID);

            totalOrcamento.ClienteNome = cliente.Nome;

            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            if (ModelState.IsValid)
            {
                db.TotalOrcamentoes.Add(totalOrcamento);
                db.SaveChanges();
                //return RedirectToAction("Index");

                return RedirectToAction("Create/" + totalOrcamento.ID,  "Orcamentos");
            }


            return RedirectToAction("Create", "Orcamento");

            //return View(totalOrcamento);
        }

        // GET: TotalOrcamentoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalOrcamento totalOrcamento = db.TotalOrcamentoes.Find(id);
            if (totalOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(totalOrcamento);
        }

        // POST: TotalOrcamentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClienteID,ValorTotal")] TotalOrcamento totalOrcamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(totalOrcamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(totalOrcamento);
        }

        // GET: TotalOrcamentoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalOrcamento totalOrcamento = db.TotalOrcamentoes.Find(id);
            if (totalOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(totalOrcamento);
        }

        // POST: TotalOrcamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TotalOrcamento totalOrcamento = db.TotalOrcamentoes.Find(id);
            db.TotalOrcamentoes.Remove(totalOrcamento);
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
