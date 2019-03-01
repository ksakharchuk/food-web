CREATE TABLE [dbo].[recipes]
(
    [id]                    INT             NOT NULL    IDENTITY (1, 1),
    [name]                  NVARCHAR (256)  NOT NULL,
    [total_weight]          DECIMAL (10, 2) NOT NULL,
    [total_proteins]        DECIMAL (10, 2) NULL,
    [total_fats]            DECIMAL (10, 2) NULL,
    [total_carbohydrates]   DECIMAL (10, 2) NULL,
    [total_energy]          DECIMAL (10, 2) NULL,
    [created_date]          DATETIME        NOT NULL    CONSTRAINT [DF_recipes_created_date] DEFAULT (getdate()),
    [last_update_date]      DATETIME        NOT NULL    CONSTRAINT [DF_recipes_last_update_date] DEFAULT (getdate()),

    CONSTRAINT [PK_recipes] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UIX_recipes_name]
    ON [dbo].[recipes]([name] ASC);
GO
