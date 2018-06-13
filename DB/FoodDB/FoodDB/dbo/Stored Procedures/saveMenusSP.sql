
CREATE PROCEDURE saveMenusSP
@menus TMenus READONLY
AS

  MERGE menus AS tgt
    USING 
      ( SELECT  *
        FROM    @menus
      ) AS src
    ON ( tgt.id = src.id )
    WHEN MATCHED THEN 
      UPDATE
        SET   name = src.name ,
              meal_period_id = src.meal_period_id ,
              service_date = src.service_date ,
              last_update_date = GETDATE()
    WHEN NOT MATCHED THEN  
      INSERT  
      ( 
        name ,
        meal_period_id ,
        service_date, 
        created_date ,
        last_update_date
        )
      VALUES
      ( 
        name ,
        meal_period_id ,
        service_date ,
        GETDATE() ,
        GETDATE()
      );
