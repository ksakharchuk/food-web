CREATE TYPE [dbo].[TIngredients] AS TABLE (
    [id]            INT             NOT NULL,
    [barcode]       CHAR (13)       NULL,
    [name]          NVARCHAR (256)  NOT NULL,
    [category_id]   INT             DEFAULT ((1)) NOT NULL,
    [proteins]      DECIMAL (10, 2) NULL,
    [fats]          DECIMAL (10, 2) NULL,
    [carbohydrates] DECIMAL (10, 2) NULL,
    [energy]        DECIMAL (10, 2) NULL
);

