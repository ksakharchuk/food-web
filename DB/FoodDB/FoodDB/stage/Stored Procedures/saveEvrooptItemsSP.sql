CREATE PROCEDURE [stage].[saveEvrooptItemsSP]
    @source_id  TINYINT = 1,
    @items     stage.TEvroopItems READONLY
AS
/*
    MERGE INTO stage.ingredients AS Target
    USING
    (
      SELECT I.barcode,
             I.name,
             I.brand,
             I.country,
             I.proteins,
             I.fats,
             I.carbohydrates,
             I.energy,
             I.energy_string,
             I.gipermall_id,
             I.gipermall_price,
             I.edostavka_id,
             I.edostavka_price
        FROM @items I
    ) AS Source
    ON (Target.barcode = Source.barcode)
    WHEN MATCHED THEN
      UPDATE SET Target.name = ISNULL(Target.name, Source.name),
                 Target.brand = ISNULL(Target.brand, Source.brand),
                 Target.country = ISNULL(Target.country, Source.country),
                 Target.proteins = ISNULL(Target.proteins, Source.proteins),
                 Target.fats = ISNULL(Target.fats, Source.fats),
                 Target.carbohydrates = ISNULL(Target.carbohydrates, Source.carbohydrates),
                 Target.energy = ISNULL(Target.energy, Source.energy),
                 Target.energy_string = ISNULL(Target.energy_string, Source.energy_string),
                 Target.edostavka_id = Source.edostavka_id,
                 Target.edostavka_price = Source.edostavka_price
    WHEN NOT MATCHED BY TARGET THEN
      INSERT
      (
        barcode,
        name,
        brand,
        country,
        proteins,
        fats,
        carbohydrates,
        energy,
        energy_string,
        edostavka_id,
        edostavka_price
      )
      VALUES
      (barcode, name, brand, country, proteins, fats, carbohydrates, energy, energy_string, edostavka_id, edostavka_price);
      */
RETURN 0
