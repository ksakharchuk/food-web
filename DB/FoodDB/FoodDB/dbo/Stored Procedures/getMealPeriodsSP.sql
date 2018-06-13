
CREATE PROCEDURE getMealPeriodsSP
AS
  SELECT mp.id, 
    mp.name
  FROM meal_periods mp
  ORDER BY id
