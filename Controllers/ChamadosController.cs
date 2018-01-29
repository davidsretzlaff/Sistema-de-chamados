using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChamadosPro.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using X.PagedList;
namespace ChamadosPro.Controllers
{
    //[Authorize]
    public class ChamadosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //[Authorize(Roles = "MASTER")]
        public ActionResult Administrador()
        {
            return View();
        }

        // Pega os chamados e joga na tela
        public ActionResult Index(int pagina = 1)
        {
            UserInfo.UserInfo infouser = new UserInfo.UserInfo();
            string usuario = infouser.GetUserInfo(17);
            string[] info = usuario.Split('|');
            

            // CRIA VARIAVEL PARA PODER ACESSAR O NOME(TIPO DE USUARIO) PARA PODER CRIAR LISTA DE CHAMADOS
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            //JOGA TODOS OS CHAMADOS PARA USUARIO MASTER
            if (currentUser.TipoUsuario == "MASTER")
            {
                var chamados = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
                .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
                .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento).OrderByDescending(i => i.IdChamado).ToPagedList(pagina, 10);

              
                return View(chamados);
            }
            //JOGA SÓ OS CHAMADOS DO USUARIO, CASO NÃO SEJA MASTER
            else
            {
                var chamados = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
                .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
                .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento).Where(x => x.RequisitanteID == User.Identity.Name).OrderByDescending(i => i.IdChamado).ToPagedList(pagina, 10);

                return View(chamados);
            }


        }

        //DETALHES DO CHAMADO
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
        
       // DETALHES DO LOG
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

        // CRIAR CHAMADO
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao");
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao");
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao");
            return View();
        }


        // CRIAR CHAMADO HTTPOST  
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
                CriarLogDefault(chamados.IdChamado, chamados.Descricao, chamados.RequisitanteID);
                //manda email para informatica com informações de um novo chamado aberto
                SendEmail("Suporte informatica",
                    " -- Um chamado foi cadastrado no sistema -- \n" + 
                    "Ticket: " + chamados.IdChamado + 
                    "\n Descrição do chamado: " + chamados.Descricao +
                    "\n Pa: " + chamados.Pa                    
                    );
                return RedirectToAction("Index");
            }
      

            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "Descricao", chamados.IdCategoria);
            ViewBag.IdStatus = new SelectList(db.Status, "IdStatus", "Descricao", chamados.IdStatus);
            ViewBag.IdSubcategoria = new SelectList(db.SubCategorias, "IdSubcategoria", "Descricao", chamados.IdSubcategoria);                     
            return View(chamados);
        }

        // LISTA DE SUBCATEGORIA
        public JsonResult GetSubCategoria(int IdCategoria)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubCategoria> SubCategoriaList = db.SubCategorias.Where(x => x.IdCategoria == IdCategoria).ToList();
            return Json(SubCategoriaList, JsonRequestBehavior.AllowGet);

        }


        public ActionResult CriarLog()
        {
            return View();
        }

        //criar interações
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarLog(int id, [Bind(Include = "IdLog,IdChamado,ResponsavelID,DataLog,Descricao")] Log log)
        {
            Chamados chamados = db.Chamados.Find(id);
            // verifica se o chamado esta em andamento
            if (chamados.IdStatus.Value == 2)
            {
                log.ResponsavelID = User.Identity.Name;
                log.IdChamado = id;
                if (ModelState.IsValid)
                {
                    log.Datalog = DateTime.Now;
                    db.Logs.Add(log);
                    db.SaveChanges();
                    return RedirectToAction("Log", new { id = log.IdChamado });
                }
            }

            return View(log);
        }

        // Criando o log default com a descrição do chamado
        public ActionResult CriarLogDefault(int id, string descricao, string responsavel)
        {
            Log logDefault = new Log();

            Chamados chamados = db.Chamados.Find(id);
            logDefault.ResponsavelID = responsavel;
            logDefault.IdChamado = id;
            logDefault.Descricao = descricao;
            if (ModelState.IsValid)
            {
                logDefault.Datalog = DateTime.Now;
                db.Logs.Add(logDefault);
                db.SaveChanges();
                return new EmptyResult();
            }
            return new EmptyResult();
        }


        // GET: Chamados/Edit/5
        public ActionResult AdicionarMaquina(int? id)
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

        //adicionar maquina ao chamado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarMaquina([Bind(Include = "IdChamado,EquipamentoID")] Chamados chamados)

        {
            ModelState.Clear();
            var recebeChamado = db.Chamados.AsNoTracking().Where(p => p.IdChamado == chamados.IdChamado).FirstOrDefault();
            
            // condição pra adicionar maquina, precisa ter responsavel e o status não pode estar como resolvido.
            if (recebeChamado.ResponsavelID != null && recebeChamado.Status.IdStatus != 3)
            {
                chamados.RequisitanteID = recebeChamado.RequisitanteID;
                chamados.ResponsavelID = recebeChamado.ResponsavelID;
                chamados.DataAbertura = recebeChamado.DataAbertura;
                chamados.IdStatus = recebeChamado.IdStatus;
                chamados.MatriculaOperador = recebeChamado.MatriculaOperador;
                chamados.Pa = recebeChamado.Pa;
                chamados.Descricao = recebeChamado.Descricao;
                chamados.IdCategoria = recebeChamado.IdCategoria;
                chamados.IdSubcategoria = recebeChamado.IdSubcategoria;
                
                var equipamento = db.Equipamentoes.SingleOrDefault(p => p.IdEquipamento == chamados.EquipamentoID);

                
                if (equipamento != null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(chamados).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.msg = "Equipamento não cadastrado";
                return View(chamados);

            }
            else
            {
                ViewBag.msg = "Esse chamado ainda não tem responsavel";
                return View(chamados);
            }

           
        }

        public ViewResult PegarChamado(int id)
        {
            var chamadoslista = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
            .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
            .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento);

            var chamados = db.Chamados.Where(p => p.IdChamado == id).FirstOrDefault();

            // verifica se o chamado já não tem um responsavel
            if (chamados.ResponsavelID == null)
            {
                // adiciona status em andamento no chamado
                chamados.IdStatus = 2;
                // adiciona o responsavel no chamado
                chamados.ResponsavelID = User.Identity.Name;

                if (ModelState.IsValid)
                {
                    db.Entry(chamados).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("Index", chamadoslista.ToList());
                }
            }  
            return View("Index",chamadoslista.ToList());
        }

        // finaliza o chamado
        public ViewResult FinalizarChamado(int id)
        {
            var chamadoslista = db.Chamados.Include(c => c.Categoria).Include(c => c.Status)
            .Include(c => c.SubCategoria).Include(c => c.UsuarioRequisitante)
            .Include(c => c.UsuarioResponsavel).Include(c => c.Equipamento);

            var chamados = db.Chamados.Where(p => p.IdChamado == id).FirstOrDefault();
            
            //verifica se existe um responsavel
            if (chamados.ResponsavelID != null)
            {
                // adiciona status resolvido 
                chamados.IdStatus = 3;
                //adiciona a data que foi finalizado
                chamados.DataFechamento = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(chamados).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("Index", chamadoslista.ToList());
                }
            }
            return View("Index", chamadoslista.ToList());
        }


        // action para mandar email depois que o chamado foi criado
        public ActionResult SendEmail(string subject, string message)
        {
            if (ModelState.IsValid)
            {
                var senderemail = new MailAddress("suporte@maxi.com.br", "Suporte chamados");
                var receiveremail = new MailAddress("davidr@maxi.com.br");

                var password = "xx003863";
                var sub = subject;
                var body = message;


                var smtp = new SmtpClient
                {
                    Host = "192.168.0.3",
                    Port = 587,
                    Credentials = new NetworkCredential(senderemail.Address, password)
                };

                MailMessage msg = new MailMessage(senderemail, receiveremail);
                msg.CC.Add("gilbertow@maxi.com.br");
                msg.CC.Add("nicholasv@maxi.com.br");
                msg.CC.Add("suporte@maxi.com.br");
                msg.Subject = subject;
                msg.Body = body;
                smtp.Send(msg);

            }

            return new EmptyResult();
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
