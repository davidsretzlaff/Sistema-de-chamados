namespace ChamadosPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requireddescricaoChamado : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chamados", "Descricao", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chamados", "Descricao", c => c.String());
        }
    }
}
