CREATE DATABASE IF NOT EXISTS gym;

USE gym;


----------------------- tables--------------------

CREATE TABLE IF NOT EXISTS Person (
    id INT AUTO_INCREMENT,
    name VARCHAR(255),
    surname VARCHAR(255),
    age INT,
    weight DOUBLE,
    totalDays INT,
    PRIMARY KEY(id)
);

CREATE TABLE IF NOT EXISTS Day (
    id INT AUTO_INCREMENT,
    name VARCHAR(255),
    person_id INT,
    FOREIGN KEY(person_id) REFERENCES Person(id),
    PRIMARY KEY(id)
);

CREATE TABLE IF NOT EXISTS Exercise (
    id INT AUTO_INCREMENT,
    name VARCHAR(255),
    times INT,
    reps INT,
    seconds INT,
    day_id INT,
    FOREIGN KEY(day_id) REFERENCES Day(id),
    PRIMARY KEY(id)
);


--------------------TRIGGER----------------------------

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


-------------------stored procedure--------------------
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

