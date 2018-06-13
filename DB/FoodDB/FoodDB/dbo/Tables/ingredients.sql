CREATE TABLE [dbo].[ingredients] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (256)  NOT NULL,
    [barcode]          CHAR (13)       NULL,
    [category_id]      INT             CONSTRAINT [DF__ingredien__categ__1273C1CD] DEFAULT ((1)) NOT NULL,
    [created_date]     DATETIME        CONSTRAINT [DF__ingredien__creat__1367E606] DEFAULT (getdate()) NOT NULL,
    [last_update_date] DATETIME        CONSTRAINT [DF__ingredien__last___145C0A3F] DEFAULT (getdate()) NOT NULL,
    [proteins]         DECIMAL (10, 2) NULL,
    [fats]             DECIMAL (10, 2) NULL,
    [carbohydrates]    DECIMAL (10, 2) NULL,
    [energy]           DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_ingredients] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ingredients_categories] FOREIGN KEY ([category_id]) REFERENCES [dbo].[categories] ([id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ingredients_name]
    ON [dbo].[ingredients]([name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ingredients_barcode]
    ON [dbo].[ingredients]([barcode] ASC) WHERE ([barcode] IS NOT NULL);

