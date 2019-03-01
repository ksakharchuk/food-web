CREATE PROCEDURE [dbo].[saveRecipeSP]
    @recipe_id              INT = 0,
    @recipe_name            NVARCHAR (256),
    @recipe_total_weight    DECIMAL (10,2),
    @ingredients            dbo.TRecipeIngredients READONLY,
    --@ingredients_delete dbo.TDataKeys READONLY,
    @param2 int
AS
    IF @recipe_id = 0
    BEGIN
        INSERT INTO dbo.recipes(name) VALUES (@recipe_name);
        SET @recipe_id = SCOPE_IDENTITY();
    END;

    ;WITH cte_target AS (
        SELECT *
        FROM dbo.recipe_ingredients
        WHERE recipe_id = @recipe_id
    )
    MERGE cte_target AS tgt
    USING 
      ( SELECT  
            ingredient_id,
            ingredient_weight
        FROM    @ingredients
      ) AS src
    ON ( tgt.ingredient_id = src.ingredient_id )
    WHEN MATCHED THEN 
      UPDATE
        SET   tgt.ingredient_weight = src.ingredient_weight,
              tgt.last_update_date = GETDATE()
    WHEN NOT MATCHED THEN  
      INSERT  
      ( 
        recipe_id,
        ingredient_id,
        ingredient_weight,
        created_date,
        last_update_date
        )
      VALUES
      ( 
        @recipe_id,
        ingredient_id,
        ingredient_weight,
        GETDATE() ,
        GETDATE()
      )
    WHEN NOT MATCHED BY SOURCE THEN DELETE;

    ;WITH cte_ingredients AS (
        SELECT 
            SUM(I.proteins * RI.ingredient_weight / 100) AS proteins,
            SUM(I.fats * RI.ingredient_weight / 100) AS fats,
            SUM(I.carbohydrates * RI.ingredient_weight / 100) AS carbohydrates,
            SUM(I.energy * RI.ingredient_weight / 100) AS energy
        FROM @ingredients RI
            JOIN dbo.ingredients I ON I.id = RI.ingredient_id
    )
    UPDATE dbo.recipes SET
        total_proteins = I.proteins,
        total_fats = I.fats,
        total_carbohydrates = I.carbohydrates,
        total_energy = I.energy,
        total_weight = @recipe_total_weight,
        last_update_date = GETDATE()
    FROM dbo.recipes R
        CROSS JOIN cte_ingredients I
    WHERE R.id = @recipe_id
RETURN 0
GO