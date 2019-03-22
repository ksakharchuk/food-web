MERGE INTO dbo.ingredients AS Target
USING (
	SELECT 
         I.barcode,
         I.name,
         I.brand,
         I.country,
         I.proteins,
         I.fats,
         I.carbohydrates,
         I.energy
	FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY I.name ORDER BY I.id) AS rn
          FROM stage.ingredients I WITH (NOLOCK)
          WHERE I.energy IS NOT NULL 
            OR I.proteins IS NOT NULL  
            OR I.fats IS NOT NULL  
            OR I.carbohydrates IS NOT NULL
          ) I
          WHERE I.rn = 1
       ) AS Source
ON (Target.barcode = Source.barcode)
WHEN MATCHED THEN
    UPDATE SET
		Target.name = ISNULL(Target.name, Source.name),
		Target.proteins = ISNULL(Target.proteins, Source.proteins),
		Target.fats = ISNULL(Target.fats, Source.fats),
		Target.carbohydrates = ISNULL(Target.carbohydrates, Source.carbohydrates),
		Target.energy = ISNULL(Target.energy, Source.energy),
        Target.last_update_date = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
INSERT (
	barcode,
	name,
	proteins,
	fats,
	carbohydrates,
	energy
) 
VALUES (
    barcode,
	name,
	proteins,
	fats,
	carbohydrates,
	energy
);