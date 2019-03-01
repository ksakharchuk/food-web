CREATE PROCEDURE [dbo].[getIngredientByIdSP]
    @id INT
AS
  SELECT 
    id AS ID,
    name AS Name,
	barcode AS Barcode,
    category_id AS CategoryID,
    proteins AS Proteins,
    fats AS Fats,
    carbohydrates AS Carbohydrates,
    energy AS Energy
  FROM ingredients i WITH (NOLOCK)
  WHERE id = @id
