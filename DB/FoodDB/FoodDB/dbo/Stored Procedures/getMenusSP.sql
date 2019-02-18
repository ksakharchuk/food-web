
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
  FROM menus m WITH (NOLOCK)
    JOIN meal_periods mp WITH (NOLOCK) ON m.meal_period_id = mp.id
    OUTER APPLY getMenuValuesFN(m.id) val
  ORDER BY service_date DESC
