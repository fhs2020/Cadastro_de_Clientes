namespace FFCJardinagens.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changeitbacktoin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "ID", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "ID", c => c.Guid(nullable: false));
        }
    }
}
