CREATE TYPE [stage].[TEvroopItems] AS TABLE (
    [Barcode]           CHAR (13)       NOT NULL,
    [Name]              NVARCHAR (256)  NOT NULL,
    [Brand]             NVARCHAR (256)  NULL,
    [Country]           NVARCHAR (256)  NULL,
    [Proteins]          NVARCHAR (256)  NULL,
    [Fats]              NVARCHAR (256)  NULL,
    [Carbohydrates]     NVARCHAR (256)  NULL,
    [Energy]            NVARCHAR (256)  NULL,
    [EnergyString]      NVARCHAR (256)  NULL,
    [ArticleId]         INT             NULL,
    [ArticlePrice]      NVARCHAR (256)  NULL
);
