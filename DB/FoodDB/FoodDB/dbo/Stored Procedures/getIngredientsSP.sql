CREATE PROCEDURE [dbo].[getIngredientsSP]
    @namePattern1 VARCHAR(10) = NULL,
    @namePattern2 VARCHAR(10) = NULL,
    @namePattern3 VARCHAR(10) = NULL,
    @namePattern4 VARCHAR(10) = NULL
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
    WHERE (name LIKE '%' + @namePattern1 + '%' OR @namePattern1 IS NULL)
        AND (name LIKE '%' + @namePattern2 + '%' OR @namePattern2 IS NULL)
        AND (name LIKE '%' + @namePattern3 + '%' OR @namePattern3 IS NULL)
        AND (name LIKE '%' + @namePattern4 + '%' OR @namePattern4 IS NULL)
    ORDER BY name

END
GO
