CREATE PROCEDURE [dbo].[getRecipesSP]
    @namePattern VARCHAR(10) = NULL
AS
BEGIN
    SELECT TOP 20
        R.id AS ID,
        R.name AS Name,
        R.total_weight AS TotalWeight,
        R.total_proteins AS TotalProteins,
        R.total_fats AS TotalFats,
        R.total_carbohydrates AS TotalCarbohydrates,
        R.total_energy AS TotalEnergy
    FROM dbo.recipes R WITH(NOLOCK)
    WHERE name LIKE '%' + @namePattern + '%' OR @namePattern IS NULL

END
GO
