DELIMITER $$

CREATE TRIGGER update_total_days
AFTER INSERT ON Day
FOR EACH ROW
BEGIN
    UPDATE Person
    SET totalDays = totalDays + 1
    WHERE id = NEW.person_id;
END$$

DELIMITER ;


-- stored procedure
DELIMITER //
CREATE PROCEDURE MostExercisesDay()
BEGIN
    SELECT day_id, COUNT(*) as exercise_count 
    FROM Exercise 
    GROUP BY day_id 
    ORDER BY exercise_count DESC 
    LIMIT 1;
END //
DELIMITER ;

