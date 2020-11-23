namespace store_api.SqlServer.DAL
{
    public static class Sql
    {
        public const string GetProducts = @"SELECT [ProductId] AS [Id]
      ,[Name]
      ,[Description]
      ,[ImageUrl]
      ,[CategoryTypeId] AS [CatergoryId]
      ,[PricePerUnit]
      ,[DateCreated]
  FROM [PetShop].[dbo].[Product]
  WHERE [DateExpired] IS NULL";
        public const string ExpireProduct = @"UPDATE [dbo].[Product]
                                              SET [DateExpired] = GETDATE()
                                              WHERE ProductId = @id";

        public const string InsertProduct = @"INSERT INTO [dbo].[Product]
           ([Name]
           ,[Description]
           ,[ImageUrl]
           ,[CategoryTypeId]
           ,[PricePerUnit]
           ,[DateCreated]
           ,[DateExpired])
     VALUES
           (@Name
           ,@Description
           ,@ImageUrl
           ,@CategoryTypeId
           ,@PricePerUnit
           ,GETDATE()
           ,NULL)
GO";
    }
}