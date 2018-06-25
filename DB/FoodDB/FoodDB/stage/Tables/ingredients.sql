CREATE TABLE [stage].[ingredients] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [barcode]       CHAR (13)       NOT NULL,
    [name]          NVARCHAR (256)  NOT NULL,
    [brand]         NVARCHAR (256)  NULL,
    [country]       NVARCHAR (256)  NULL,
    [proteins]      DECIMAL (10, 2) NULL,
    [fats]          DECIMAL (10, 2) NULL,
    [carbohydrates] DECIMAL (10, 2) NULL,
    [energy]        DECIMAL (10, 2) NULL,
    [gipermall_id]  INT             NULL,
    [price]         DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_stage_ingredients] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_stage_ingredients_barcode] UNIQUE NONCLUSTERED ([barcode] ASC)
);



