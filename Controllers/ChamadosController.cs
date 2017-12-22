using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChamadosPro.Models;

namespace ChamadosPro.Controllers
{
    public class ChamadosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chamados
        public ActionResult Index()
        {
            var chamados = db.Chamados.Include(c => c.Categoria).Include(c => c.Status).Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante).Include(c => c.UsuarioResponsavel);
            return View(chamados.ToList());
        }

        // GET: Chamados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamados chamados = db.Chamados.Find(id);
            if (chamados == null)
            {
                return HttpNotFound();
            }
            return View(chamados);
        }

        // GET: Chamados/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao");
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao");
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao");
            return View();
        }

        // POST: Chamados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdChamado,Descricao,Pa,IdCategoria,IdSubcategoria,IdStatus,MatriculaOperador,DataAbertura,DataFechamento,RequisitanteID,ResponsavelID")] Chamados chamados)
        {
            chamados.RequisitanteID = User.Identity.Name;
            if (ModelState.IsValid)
            {
                chamados.DataAbertura = DateTime.Now;
                chamados.IdStatus = 1;
                db.Chamados.Add(chamados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao", chamados.IdCategoria);
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao", chamados.IdStatus);
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao", chamados.IdSubcategoria);                     
            return View(chamados);
        }
        public JsonResult GetSubCategoria(int IdCategoria)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubCategoria> SubCategoriaList = db.SubCategorias.Where(x => x.IdCategoria == IdCategoria).ToList();
            return Json(SubCategoriaList, JsonRequestBehavior.AllowGet);

        }

        // GET: Chamados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamados chamados = db.Chamados.Find(id);
            if (chamados == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao", chamados.IdCategoria);
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao", chamados.IdStatus);
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao", chamados.IdSubcategoria);

            return View(chamados);
        }

        // POST: Chamados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdChamado,Descricao,Pa,IdCategoria,IdSubcategoria,IdStatus,MatriculaOperador,DataAbertura,DataFechamento,RequisitanteID,ResponsavelID")] Chamados chamados)
        {
            //Cria variavel para receber os dados do banco, para depois armazenar os 
            //dados já existente, sem precisar mostrar para o usuario
            var recebeChamado = db.Chamados.AsNoTracking().Where(p => p.IdChamado == chamados.IdChamado).FirstOrDefault();             
            chamados.RequisitanteID = recebeChamado.RequisitanteID;
            chamados.DataAbertura = recebeChamado.DataAbertura;            
            

            if (recebeChamado.ResponsavelID == null)
                chamados.ResponsavelID = User.Identity.Name;
            else { chamados.ResponsavelID = recebeChamado.ResponsavelID; }

            if (chamados.IdStatus.Value == 3)
            {
                chamados.DataFechamento = DateTime.Now;
            }

            else { chamados.DataFechamento = recebeChamado.DataFechamento; }   

            if (ModelState.IsValid)
            {               
                db.Entry(chamados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao", chamados.IdCategoria);
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao", chamados.IdStatus);
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao", chamados.IdSubcategoria);
            return View(chamados);
        }

        // GET: Chamados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamados chamados = db.Chamados.Find(id);
            if (chamados == null)
            {
                return HttpNotFound();
            }
            return View(chamados);
        }

        // POST: Chamados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chamados chamados = db.Chamados.Find(id);
            db.Chamados.Remove(chamados);
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
