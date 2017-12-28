using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChamadosPro.Models;
using System.Data.Entity;
using System.Web.Mvc.Html;

namespace ChamadosPro.Controllers
{
    [Authorize(Roles = "adm")]
    public class ChamadosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chamados
        public ActionResult Index()
       {
            var chamados = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
                .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
                .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento);
            
            return View(chamados.ToList());
        }

        // GET: Chamados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamados chamados = db.Chamados.SingleOrDefault(c => c.IdChamado == id);
            if (chamados == null)
            {
                return HttpNotFound();
            }
            return View(chamados);
        }
        public ActionResult CriarLog()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarLog(int id,[Bind(Include = "IdLog,IdChamado,ResponsavelID,DataLog,Descricao")] Log log)
        {
            log.ResponsavelID = User.Identity.Name;
            log.IdChamado = id;
            if (ModelState.IsValid)
            {
                log.Datalog = DateTime.Now;
                db.Logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Log",new { id = log.IdChamado });                
            }
            return View(log);
        }


        public ActionResult Log(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var log = db.Logs.Include(x => x.Chamados).Where(c => c.IdChamado == id);

            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }
        // GET: Chamados/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao");
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao");
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao");
            return View();
        }


            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdChamado,Descricao,Pa,IdCategoria,IdSubcategoria,IdStatus,MatriculaOperador,DataAbertura,DataFechamento,RequisitanteID,ResponsavelID")] Chamados chamados)
        {
            chamados.RequisitanteID = User.Identity.Name;
            if (ModelState.IsValid)
            {
                chamados.DataAbertura = DateTime.Now;
                chamados.EquipamentoID = null;
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

        //// GET: Chamados/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Chamados chamados = db.Chamados.Find(id);
        //    if (chamados == null)
        //    {
        //        return HttpNotFound();
        //    }
          
        //    ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao", chamados.IdCategoria);
        //    ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao", chamados.IdStatus);
        //    ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao", chamados.IdSubcategoria);

        //    return View(chamados);
        //}


        public ViewResult PegarChamado(int id)
        {
            var chamadoslista = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
            .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
            .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento);

            var chamados = db.Chamados.Where(p => p.IdChamado == id).FirstOrDefault();

            chamados.IdStatus = 2;

            if (chamados.ResponsavelID == null)
                chamados.ResponsavelID = User.Identity.Name;

            if (chamados.IdStatus.Value == 3)
            {
                chamados.DataFechamento = DateTime.Now;
            }


            if (ModelState.IsValid)
            {
                db.Entry(chamados).State = EntityState.Modified;
                db.SaveChanges();
                return View("Index",chamadoslista.ToList());
            }

            return View("Index",chamadoslista.ToList());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IdChamado,Descricao,Pa,IdCategoria,IdSubcategoria,IdStatus,MatriculaOperador,DataAbertura,DataFechamento,RequisitanteID,ResponsavelID")] Chamados chamados)
        //{
        //    //Cria variavel para receber os dados do banco, para depois armazenar os 
        //    //dados já existente, sem precisar mostrar para o usuario
        //    var recebeChamado = db.Chamados.AsNoTracking().Where(p => p.IdChamado == chamados.IdChamado).FirstOrDefault();             
        //    chamados.RequisitanteID = recebeChamado.RequisitanteID;
        //    chamados.DataAbertura = recebeChamado.DataAbertura;
        //    chamados.IdStatus = 2;
        //    chamados.MatriculaOperador = recebeChamado.MatriculaOperador;
        //    chamados.Pa = recebeChamado.Pa;
        //    chamados.Descricao = recebeChamado.Descricao;
        //    chamados.IdCategoria = recebeChamado.IdCategoria;
        //    chamados.IdSubcategoria = recebeChamado.IdSubcategoria;          
            

        //    if (recebeChamado.ResponsavelID == null)
        //        chamados.ResponsavelID = User.Identity.Name;
        //    else { chamados.ResponsavelID = recebeChamado.ResponsavelID; }

        //    if (chamados.IdStatus.Value == 3)
        //    {
        //        chamados.DataFechamento = DateTime.Now;
        //    }

        //    else { chamados.DataFechamento = recebeChamado.DataFechamento; }   

        //    if (ModelState.IsValid)
        //    {               
        //        db.Entry(chamados).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Chamados chamados = db.Chamados.Find(id);
        //    if (chamados == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(chamados);
        //}

        //// POST: Chamados/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Chamados chamados = db.Chamados.Find(id);
        //    db.Chamados.Remove(chamados);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
