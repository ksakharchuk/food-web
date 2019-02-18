
CREATE PROCEDURE getMenuItemsSP
@menu_id int
AS
  SELECT mi.id, 
    mi.ingredient_id,
    mi.weight
  FROM menu_items mi WITH (NOLOCK)
  ORDER BY mi.id
