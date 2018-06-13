
CREATE PROCEDURE deleteMenuItemsSP
@ids TDataKeys READONLY
AS

  DELETE FROM menu_items
  WHERE id IN (SELECT id
               FROM @ids)
