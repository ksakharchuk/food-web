CREATE TYPE [dbo].[TRecipeIngredients] AS TABLE
(
    [ingredient_id]     INT             NOT NULL    PRIMARY KEY, 
    [ingredient_weight] DECIMAL (10, 2) NOT NULL
);
