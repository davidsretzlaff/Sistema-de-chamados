namespace ChamadosPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTipoUsuarioUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TipoUsuario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TipoUsuario");
        }
    }
}
