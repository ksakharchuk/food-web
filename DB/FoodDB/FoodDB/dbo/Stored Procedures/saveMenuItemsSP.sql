
CREATE PROCEDURE saveMenuItemsSP
@menu_items TMenuItems READONLY
AS

  MERGE menu_items AS tgt
    USING 
      ( SELECT  *
        FROM    @menu_items
      ) AS src
    ON ( tgt.id = src.id )
    WHEN MATCHED THEN 
      UPDATE
        SET   menu_id = src.menu_id ,
              ingredient_id = src.ingredient_id ,
              weight = src.weight ,
              last_update_date = GETDATE()
    WHEN NOT MATCHED THEN  
      INSERT  
      ( 
        menu_id ,
        ingredient_id ,
        weight, 
        created_date ,
        last_update_date
        )
      VALUES
      ( 
        menu_id ,
        ingredient_id ,
        weight ,
        GETDATE() ,
        GETDATE()
      );
