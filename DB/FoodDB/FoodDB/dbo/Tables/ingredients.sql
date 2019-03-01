CREATE TABLE [dbo].[ingredients] (
    [id]                INT             NOT NULL    IDENTITY (1, 1),
    [name]              NVARCHAR (256)  NOT NULL,
    [barcode]           CHAR (13)       NULL,
    [category_id]       INT             NOT NULL    CONSTRAINT [DF_ingredients_category_id_1] DEFAULT (1),
    [created_date]      DATETIME        NOT NULL    CONSTRAINT [DF_ingredients_created_date] DEFAULT (getdate()),
    [last_update_date]  DATETIME        NOT NULL    CONSTRAINT [DF_ingredients_last_update_date] DEFAULT (getdate()),
    [proteins]          DECIMAL (10, 2) NULL,
    [fats]              DECIMAL (10, 2) NULL,
    [carbohydrates]     DECIMAL (10, 2) NULL,
    [energy]            DECIMAL (10, 2) NULL,

    CONSTRAINT [PK_ingredients] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ingredients_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[categories] ([id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ingredients_name]
    ON [dbo].[ingredients]([name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ingredients_barcode]
    ON [dbo].[ingredients]([barcode] ASC) WHERE ([barcode] IS NOT NULL);

