CREATE PROCEDURE [dbo].[getIngredientsSP]
    @namePattern VARCHAR(10) = NULL
AS
BEGIN
    SELECT TOP 20
        id AS ID,
        name AS Name,
        barcode AS Barcode,
        category_id AS CategoryID,
        proteins AS Proteins,
        fats AS Fats,
        carbohydrates AS Carbohydrates,
        energy AS Energy
    FROM ingredients i WITH (NOLOCK)
    WHERE name LIKE '%' + @namePattern + '%' OR @namePattern IS NULL
    ORDER BY name

END
GO
