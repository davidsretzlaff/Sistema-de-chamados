namespace ChamadosPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Iniciando : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "dbo.Chamados",
                c => new
                    {
                        IdChamado = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        Pa = c.Int(nullable: false),
                        IdCategoria = c.Int(nullable: false),
                        IdSubcategoria = c.Int(nullable: false),
                        IdStatus = c.Int(),
                        MatriculaOperador = c.Int(nullable: false),
                        DataAbertura = c.DateTime(nullable: false),
                        DataFechamento = c.DateTime(),
                        RequisitanteID = c.String(maxLength: 128),
                        ResponsavelID = c.String(maxLength: 128),
                        EquipamentoID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdChamado)
                .ForeignKey("dbo.Categorias", t => t.IdCategoria, cascadeDelete: true)
                .ForeignKey("dbo.Equipamentoes", t => t.EquipamentoID)
                .ForeignKey("dbo.Status", t => t.IdStatus)
                .ForeignKey("dbo.SubCategorias", t => t.IdSubcategoria, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.RequisitanteID)
                .ForeignKey("dbo.AspNetUsers", t => t.ResponsavelID)
                .Index(t => t.IdCategoria)
                .Index(t => t.IdSubcategoria)
                .Index(t => t.IdStatus)
                .Index(t => t.RequisitanteID)
                .Index(t => t.ResponsavelID)
                .Index(t => t.EquipamentoID);
            
            CreateTable(
                "dbo.Equipamentoes",
                c => new
                    {
                        IdEquipamento = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IdEquipamento);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatus = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.IdStatus);
            
            CreateTable(
                "dbo.SubCategorias",
                c => new
                    {
                        IdSubcategoria = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        IdCategoria = c.Int(),
                    })
                .PrimaryKey(t => t.IdSubcategoria)
                .ForeignKey("dbo.Categorias", t => t.IdCategoria)
                .Index(t => t.IdCategoria);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        IdLog = c.Int(nullable: false, identity: true),
                        IdChamado = c.Int(nullable: false),
                        ResponsavelID = c.String(maxLength: 128),
                        Datalog = c.DateTime(nullable: false),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.IdLog)
                .ForeignKey("dbo.Chamados", t => t.IdChamado, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ResponsavelID)
                .Index(t => t.IdChamado)
                .Index(t => t.ResponsavelID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ViewChamadoes",
                c => new
                    {
                        IdViewChamado = c.Int(nullable: false, identity: true),
                        Chamado_IdChamado = c.Int(),
                        Equipamento_IdEquipamento = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdViewChamado)
                .ForeignKey("dbo.Chamados", t => t.Chamado_IdChamado)
                .ForeignKey("dbo.Equipamentoes", t => t.Equipamento_IdEquipamento)
                .Index(t => t.Chamado_IdChamado)
                .Index(t => t.Equipamento_IdEquipamento);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViewChamadoes", "Equipamento_IdEquipamento", "dbo.Equipamentoes");
            DropForeignKey("dbo.ViewChamadoes", "Chamado_IdChamado", "dbo.Chamados");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Logs", "ResponsavelID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Logs", "IdChamado", "dbo.Chamados");
            DropForeignKey("dbo.Chamados", "ResponsavelID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chamados", "RequisitanteID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chamados", "IdSubcategoria", "dbo.SubCategorias");
            DropForeignKey("dbo.SubCategorias", "IdCategoria", "dbo.Categorias");
            DropForeignKey("dbo.Chamados", "IdStatus", "dbo.Status");
            DropForeignKey("dbo.Chamados", "EquipamentoID", "dbo.Equipamentoes");
            DropForeignKey("dbo.Chamados", "IdCategoria", "dbo.Categorias");
            DropIndex("dbo.ViewChamadoes", new[] { "Equipamento_IdEquipamento" });
            DropIndex("dbo.ViewChamadoes", new[] { "Chamado_IdChamado" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Logs", new[] { "ResponsavelID" });
            DropIndex("dbo.Logs", new[] { "IdChamado" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SubCategorias", new[] { "IdCategoria" });
            DropIndex("dbo.Chamados", new[] { "EquipamentoID" });
            DropIndex("dbo.Chamados", new[] { "ResponsavelID" });
            DropIndex("dbo.Chamados", new[] { "RequisitanteID" });
            DropIndex("dbo.Chamados", new[] { "IdStatus" });
            DropIndex("dbo.Chamados", new[] { "IdSubcategoria" });
            DropIndex("dbo.Chamados", new[] { "IdCategoria" });
            DropTable("dbo.ViewChamadoes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SubCategorias");
            DropTable("dbo.Status");
            DropTable("dbo.Equipamentoes");
            DropTable("dbo.Chamados");
            DropTable("dbo.Categorias");
        }
    }
}
