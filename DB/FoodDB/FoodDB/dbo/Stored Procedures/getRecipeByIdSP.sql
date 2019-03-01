CREATE PROCEDURE [dbo].[getRecipeByIdSP]
    @id int
AS
BEGIN
    SELECT 
        R.id AS ID,
        R.name AS Name,
        R.total_weight AS TotalWeight,
        R.total_proteins AS TotalProteins,
        R.total_fats AS TotalFats,
        R.total_carbohydrates AS TotalCarbohydrates,
        R.total_energy AS TotalEnergy
    FROM dbo.recipes R WITH(NOLOCK)
    WHERE id = @id

    SELECT RI.ingredient_id AS IngredientID,
           RI.ingredient_weight AS IngredientWeight
    FROM dbo.recipe_ingredients RI WITH(NOLOCK)
    WHERE recipe_id = @id
END
GO