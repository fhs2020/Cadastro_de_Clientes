using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FFCJardinagens.Models;

namespace FFCJardinagens.Controllers
{
    public class OrcamentosController : Controller
    {
        private FFCJardinagensContext db = new FFCJardinagensContext();

        // GET: Orcamentos
        public ActionResult Index()
        {
            return View(db.Orcamentoes.ToList());
        }

        // GET: Orcamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        // GET: Orcamentos/Create
        public ActionResult Create(int? ID)
        {
            var id = RouteData.Values["id"];

            var orcamentoID = Convert.ToInt32(id);

            var totalOrcamento = db.TotalOrcamentoes.Find(orcamentoID);

            var clientes = db.Clientes.ToList();

            var cliente = new Cliente();

            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            var clienteID = totalOrcamento.ClienteID;

            cliente = db.Clientes.Find(clienteID);

            ViewBag.ClienteID = new SelectList(clientes, "ID", "Nome", cliente).SelectedValue;

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            var items = db.Orcamentoes.Where(x => x.TotalOrcamentoID == totalOrcamento.ID).ToList();

            if (items != null && items.Count() > 0)
            {
                ViewBag.cartCount = items.Count();
            }
            else
            {
                ViewBag.cartCount = 0;
            }



            var orcamento = new Orcamento();

            orcamento.ClienteID = clienteID;
            orcamento.ClienteNome = cliente.Nome;
            orcamento.TotalOrcamentoID = totalOrcamento.ID;

            return View(orcamento);
        }

        // POST: Orcamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int id, int totalOrcamentoID, string produtoTotal, int produtoUnidade, string descriminacao, string porte)
        {
            Orcamento orcamento = new Orcamento();

            orcamento.TotalOrcamentoID = totalOrcamentoID;

            var productTotal = decimal.Parse(produtoTotal);

            orcamento.ProdutoTotal = productTotal;

            orcamento.ProdutoUnidade = produtoUnidade;
            orcamento.Descriminação = descriminacao;
            orcamento.Porte = porte;
            orcamento.ValorTotal = (productTotal * produtoUnidade);


            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            decimal countTotal = 0;

       
                var total = (orcamento.ProdutoUnidade * orcamento.ProdutoTotal);

                orcamento.ValorTotal = total;

                var listaOrcamentos = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamento.TotalOrcamentoID).ToList();

                foreach (var item in listaOrcamentos)
                {
                    countTotal += (item.ProdutoTotal * item.ProdutoUnidade);
                }

                var totalConvertido = Convert.ToDecimal(total);

                countTotal += totalConvertido;

                var totalOrcamento = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

                if (totalOrcamento != null)
                {
                    totalOrcamento.ValorTotal = countTotal;

                    db.Entry(totalOrcamento).State = EntityState.Modified;
                    db.Orcamentoes.Add(orcamento);
                    db.SaveChanges();
                }
        

            var contador = AddCart(orcamento);

            ViewBag.cartCount = contador;

            var listaOrcamentosFinal = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamento.TotalOrcamentoID).ToList();

            ViewBag.ListaOrcamentos = listaOrcamentosFinal;

            return RedirectToAction("Index");
        }


        public ActionResult Print(int? totalOrcamentoID)
        {
            var id = RouteData.Values["id"];

            var orcamentoID = Convert.ToInt32(id);



            var totalOrcamentos = db.TotalOrcamentoes.Find(orcamentoID);

            ViewBag.Cliente = totalOrcamentos.ClienteNome;

            ViewBag.ValorTotal = totalOrcamentos.ValorTotal;

            var listaOrcamentos = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamentoID).ToList();

            return View(listaOrcamentos);
        }


        public IList<Orcamento> getAllItems()
        {
            IList<Orcamento> orcamentos = new List<Orcamento>();
            orcamentos = db.Orcamentoes.ToList();
            return orcamentos;
        }

        public int AddCart(Orcamento orcamentoObj)
        {

            //Orcamento orcamento = new Orcamento()
            //{
            //    ClienteID = orcamentoObj.ClienteID,
            //    ValorTotal = orcamentoObj.ValorTotal,
            //    DataOrcamento = DateTime.Now,
            //    TotalOrcamentoID = orcamentoObj.TotalOrcamentoID,
            //    Descriminação = orcamentoObj.Descriminação,
            //    Quantidade = orcamentoObj.Quantidade,
            //    ProdutoTotal = orcamentoObj.ProdutoTotal,
            //    ProdutoUnidade = orcamentoObj.ProdutoUnidade
            //};

            //db.Orcamentoes.Add(orcamento);
            //db.SaveChanges();

            int count = db.Orcamentoes.Where(s => s.TotalOrcamentoID == orcamentoObj.TotalOrcamentoID).Count();

            return count;
        }

        [HttpPost]
        public ActionResult GetCartItems(int? totalOrcamentoID)
        {

            string x = Request.QueryString["id"];

            var queryString = HttpContext.Request.QueryString.Get("id");


            var idTotalOrcamento = Convert.ToInt32(x);

            decimal sum = 0;

            //var orcamento = db.Orcamentoes.Find(orcamentoID);

            var GetItems = db.Orcamentoes.Where(s => s.TotalOrcamentoID == totalOrcamentoID).ToList();
            //var GetCartItem = from itemList in db.Orcamentoes where GetItemsId.Contains(itemList.ClienteID) select itemList;

            foreach (var totalsum in GetItems)
            {
                sum = sum + totalsum.ProdutoTotal;
            }

            ViewBag.ValorTotal = sum;

            //return PartialView("_cartItem", GetCartItem);

            //return Json(GetCartItem);

            ViewBag.ListaOrcamentos = GetItems;

            //return RedirectToAction("Create", new { id = totalOrcamentoID });
            //var orcamento = new Orcamento();

            return Json(GetItems);

        }

        public ActionResult DeleteCart(int itemId)
        {
            Orcamento removeCart = db.Orcamentoes.Find(itemId);

            var totalOrcamento = db.TotalOrcamentoes.Find(removeCart.TotalOrcamentoID);

            totalOrcamento.ValorTotal -= removeCart.ProdutoTotal;

            db.Entry(totalOrcamento).State = EntityState.Modified;
            db.Orcamentoes.Remove(removeCart);
            db.SaveChanges();

            return GetCartItems(totalOrcamento.ID);
        }

        [HttpPost]
        public ActionResult GetOrcamentoById(int orcamentoID)
        {
            Orcamento editarOrcamento = db.Orcamentoes.Find(orcamentoID);


            var valor = string.Format("{0:0,0.00}", editarOrcamento.ProdutoTotal);

            ViewBag.Valor = valor;
            //var totalOrcamento = db.TotalOrcamentoes.Find(editarOrcamento.TotalOrcamentoID);

            //totalOrcamento.ValorTotal -= editarOrcamento.ProdutoTotal;


            return Json(editarOrcamento);
        }


        // GET: Orcamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        [HttpPost]
        public ActionResult Edit(int id, int totalOrcamentoID, string produtoTotal, int produtoUnidade, string descriminacao, string porte)
        {
            Orcamento orcamento = db.Orcamentoes.Find(id);

            orcamento.TotalOrcamentoID = totalOrcamentoID;

            var productTotal = decimal.Parse(produtoTotal);

            orcamento.ProdutoTotal = productTotal;
         
            
            orcamento.ProdutoUnidade = produtoUnidade;
            orcamento.Descriminação = descriminacao;
            orcamento.Porte = porte;
            orcamento.ValorTotal = (productTotal * produtoUnidade);

            var orcamentoTotal = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

            var checkValue = orcamentoTotal.ValorTotal;

                orcamento.ValorTotal = (orcamento.ProdutoTotal * orcamento.ProdutoUnidade);

                db.Entry(orcamento).State = EntityState.Modified;
                db.SaveChanges();

                var todosItenns = db.Orcamentoes.Where(x => x.TotalOrcamentoID == totalOrcamentoID).ToList();

                Decimal contador = 0;

                foreach (var item in todosItenns)
                {
                    contador += (item.ProdutoTotal * item.ProdutoUnidade);
                }

                orcamentoTotal.ValorTotal = contador;

                db.Entry(orcamento).State = EntityState.Modified;
                db.Entry(orcamentoTotal).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");    
        }

        // GET: Orcamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }


        [HttpPost]
        public string PostSendGmail(int id)
        {
            var order = db.TotalOrcamentoes.Find(id);

            var listaItemsDoPedido = db.Orcamentoes.Where(x => x.TotalOrcamentoID == id);

            var cliente = db.Clientes.Find(order.ClienteID);



            decimal total = 0;



            string textBody = "<html><head><h2>F.F.C Jardinagens Orçamento</h2><h4>(62) 3249-1001 / 9 9269-9179</h4><h4>firminojardineiro@outlook.com</h4><h4>MANUTENÇÃO EM JARDINS E LIMPEZA DE LOTES</h4><h4>Cliente: " + cliente.Nome + "</h4></head><body><table class='table'><tr><th>Quant.</th> <th>Descrição</th> <th>Porte</th> <th>Unit.</th> <th>Total(R$)</th> <th></th></tr>";

            foreach (var item in listaItemsDoPedido)
            {

                textBody += "<tr><td style='border-bottom:1pt solid black'>  " + item.ProdutoUnidade + "</td> " + "<td style='border-bottom:1pt solid black'> " + item.Descriminação + "</td>" + "<td style='border-bottom:1pt solid black'> " + item.Porte + "</td> " + "<td style='border-bottom:1pt solid black'> " + item.ProdutoTotal + "</td>  " + "<td style='border-bottom:1pt solid black'> " + item.ValorTotal + "</td></tr>";

                total += item.ValorTotal.Value;
            }
            textBody += "<tr><td style='border-bottom:1pt solid black'>Valor total: </td><td style='border-bottom:1pt solid black'></td><td style='border-bottom:1pt solid black'></td><td style='border-bottom:1pt solid black'></td><td colspan='10' style='border-bottom:1pt solid black'><h4> R$ <b>" + total + "</b></h4></tr></table></body></html>";

            //var usuarioId = User.Identity.GetUserId();
            //var usuario = db.Users.Find(usuarioId);

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("ffc.jardineiro@gmail.com", "senha do email aqui!");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            //can be obtained from your model
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("ffc.jardineiro@gmail.com");
            msg.To.Add(new MailAddress("firminojardineiro@outlook.com"));
            msg.To.Add(new MailAddress(cliente.Email));
            //msg.To.Add(new MailAddress("flavioh007@gmail.com"));

            msg.Subject = "Pedido Enviado com sucesso";
            msg.IsBodyHtml = true;
            msg.Body = string.Format(textBody);


            try
            {
                client.Send(msg);
                return "OK";
            }
            catch (Exception ex)
            {

                return "error:" + ex.ToString();
            }
        }





        // POST: Orcamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Orcamento orcamento = db.Orcamentoes.Find(id);

            var orcamentoTotal = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

            var valorAtual = orcamento.ValorTotal;

            var removedItem = (orcamento.ProdutoTotal * orcamento.ProdutoUnidade);

            var itemToRemoveValue = (orcamentoTotal.ValorTotal -= orcamento.ValorTotal);

            orcamentoTotal.ValorTotal = itemToRemoveValue;


            db.Entry(orcamentoTotal).State = EntityState.Modified;
            db.Orcamentoes.Remove(orcamento);
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
