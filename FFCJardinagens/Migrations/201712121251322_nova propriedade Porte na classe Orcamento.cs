namespace FFCJardinagens.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class novapropriedadePortenaclasseOrcamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orcamentoes", "Porte", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orcamentoes", "Porte");
        }
    }
}
