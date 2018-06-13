
CREATE PROCEDURE deleteIngredientsSP
@ids TDataKeys READONLY
AS

  DELETE FROM ingredients
  WHERE id IN (SELECT id
               FROM @ids)
