
CREATE FUNCTION getMenuValuesFN(@menu_id int)
RETURNS TABLE
AS RETURN
  SELECT CAST(SUM(mi.weight*i.proteins)/100 AS decimal(10,2)) AS proteins,
    CAST(SUM(mi.weight*i.fats)/100 AS decimal(10,2)) AS fats,
    CAST(SUM(mi.weight*i.carbohydrates)/100 AS decimal(10,2)) AS carbohydrates,
    CAST(SUM(mi.weight*i.energy)/100 AS decimal(10,2)) AS energy
  FROM menu_items mi
    JOIN ingredients i ON mi.ingredient_id = i.id
  WHERE mi.menu_id = @menu_id
