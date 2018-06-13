
CREATE PROCEDURE deleteMenusSP
@ids TDataKeys READONLY
AS
  SET XACT_ABORT ON
  
  BEGIN TRAN
  
  DELETE FROM menu_items
  WHERE menu_id IN (SELECT id
                    FROM @ids)

  DELETE FROM menus
  WHERE id IN (SELECT id
               FROM @ids)
  COMMIT
