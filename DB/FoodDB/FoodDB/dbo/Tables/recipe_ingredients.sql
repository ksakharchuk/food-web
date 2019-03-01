CREATE TABLE [dbo].[recipe_ingredients]
(
    [id]                    INT             NOT NULL    IDENTITY (1, 1),
    [recipe_id]             INT             NOT NULL,
    [ingredient_id]         INT             NOT NULL,
    [ingredient_weight]     DECIMAL (10, 2) NOT NULL,
    [created_date]          DATETIME        NOT NULL    CONSTRAINT [DF_recipe_ingredients_created_date] DEFAULT (getdate()),
    [last_update_date]      DATETIME        NOT NULL    CONSTRAINT [DF_recipe_ingredients_update_date] DEFAULT (getdate()),

    CONSTRAINT [PK_recipe_ingredients] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_recipe_ingredients_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients] ([id]),
    CONSTRAINT [FK_recipe_ingredients_recipes] FOREIGN KEY ([recipe_id]) REFERENCES [dbo].[recipes] ([id])
)
