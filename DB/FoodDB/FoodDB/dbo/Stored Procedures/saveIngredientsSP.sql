
create PROCEDURE [dbo].[saveIngredientsSP]
@ingredients TIngredients READONLY
AS

  MERGE ingredients AS tgt
    USING 
      ( SELECT  *
        FROM    @ingredients
      ) AS src
    ON ( tgt.id = src.id )
    WHEN MATCHED THEN 
      UPDATE
        SET   name = src.name ,
              barcode = src.barcode ,
              category_id = src.category_id ,
              proteins = src.proteins ,
              fats = src.fats ,
              carbohydrates = src.carbohydrates ,
              energy = src.energy ,
              last_update_date = GETDATE()
    WHEN NOT MATCHED THEN  
      INSERT  
      ( 
        name ,
        barcode ,
        category_id ,
        created_date ,
        last_update_date ,
        proteins ,
        fats ,
        carbohydrates ,
        energy
        )
      VALUES
      ( 
        name ,
        barcode ,
        category_id ,
        GETDATE() ,
        GETDATE() ,
        proteins ,
        fats ,
        carbohydrates ,
        energy
      );
