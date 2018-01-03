namespace FFCJardinagens.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            //    CreateTable(
            //        "dbo.Carts",
            //        c => new
            //            {
            //                ID = c.Int(nullable: false, identity: true),
            //                ClienteID = c.Int(nullable: false),
            //                OrcamentoID = c.Int(nullable: false),
            //            })
            //        .PrimaryKey(t => t.ID);

            //    CreateTable(
            //        "dbo.Clientes",
            //        c => new
            //            {
            //                ID = c.Int(nullable: false, identity: true),
            //                Empresa = c.String(unicode: false),
            //                Nome = c.String(unicode: false),
            //                Email = c.String(unicode: false),
            //                Telefone = c.String(unicode: false),
            //                Celular = c.String(unicode: false),
            //                CNPJ = c.String(unicode: false),
            //                Endereco = c.String(unicode: false),
            //                Cidade = c.String(unicode: false),
            //                UF = c.String(unicode: false),
            //                CEP = c.String(unicode: false),
            //            })
            //        .PrimaryKey(t => t.ID);

            //    CreateTable(
            //        "dbo.Orcamentoes",
            //        c => new
            //            {
            //                ID = c.Int(nullable: false, identity: true),
            //                ClienteID = c.Int(nullable: false),
            //                ClienteNome = c.String(unicode: false),
            //                TotalOrcamentoID = c.Int(nullable: false),
            //                Quantidade = c.Int(nullable: false),
            //                Descriminação = c.String(unicode: false),
            //                ProdutoUnidade = c.Int(nullable: false),
            //                ProdutoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
            //                ValorTotal = c.Decimal(precision: 18, scale: 2),
            //                DataOrcamento = c.DateTime(precision: 0),
            //            })
            //        .PrimaryKey(t => t.ID)
            //        .ForeignKey("dbo.TotalOrcamentoes", t => t.TotalOrcamentoID, cascadeDelete: true)
            //        .Index(t => t.TotalOrcamentoID);

            //    CreateTable(
            //        "dbo.TotalOrcamentoes",
            //        c => new
            //            {
            //                ID = c.Int(nullable: false, identity: true),
            //                ClienteID = c.Int(nullable: false),
            //                ClienteNome = c.String(unicode: false),
            //                ValorTotal = c.Decimal(precision: 18, scale: 2),
            //                DataOrcamento = c.DateTime(precision: 0),
            //            })
            //        .PrimaryKey(t => t.ID);

            //}
        }
        public override void Down()
        {
            DropForeignKey("dbo.Orcamentoes", "TotalOrcamentoID", "dbo.TotalOrcamentoes");
            DropIndex("dbo.Orcamentoes", new[] { "TotalOrcamentoID" });
            DropTable("dbo.TotalOrcamentoes");
            DropTable("dbo.Orcamentoes");
            DropTable("dbo.Clientes");
            DropTable("dbo.Carts");
        }
    }
}
