
CREATE PROCEDURE getMenusSP
AS
  SELECT m.id, 
    m.name, 
    m.meal_period_id, 
    service_date, 
    val.proteins, 
    val.fats, 
    val.carbohydrates, 
    val.energy
  FROM menus m
    JOIN meal_periods mp ON m.meal_period_id = mp.id
    OUTER APPLY getMenuValuesFN(m.id) val
  ORDER BY service_date DESC
