using System;
using System.Data;
using MySql.Data.MySqlClient;
using WinFormsApp1.Model;

namespace WinFormsApp1.DAO
{   
    //classe que conté la connexió a la base de dades
    public class PersonDAO
    {
        private MySqlConnection connection;

        public PersonDAO(MySqlConnection connection)
        {
            this.connection = connection;
        }
        //mètode per a guardar la persona a la base de dades
        public void SavePerson(Person person)
        {
            // Codi per a inserir la informació de la persona a la taula "Person" de la base de dades
            string query = "INSERT INTO Person (name, surname, age, weight, totalDays) VALUES (@name, @surname, @age, @weight, @totalDays)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", person.Name);
            cmd.Parameters.AddWithValue("@surname", person.Surname);
            cmd.Parameters.AddWithValue("@age", person.Age);
            cmd.Parameters.AddWithValue("@weight", person.Weight);
            cmd.Parameters.AddWithValue("@totalDays", person.TotalDays);

            cmd.ExecuteNonQuery();
            // Bucle per a guardar els dies relacionats amb la persona a la base de dades
            foreach (var day in person.Days)
            {
                DayDAO dayDAO = new DayDAO(connection);
                dayDAO.SaveDay(day, cmd.LastInsertedId);
            }
        }
    }
    //classe que conté la connexió a la base de dades
    public class DayDAO
    {
        private MySqlConnection connection;

        public DayDAO(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void SaveDay(Dia day, long personId)
        {
            // Codi per a inserir la informació del dia a la taula "Day" de la base de dades
            string query = "INSERT INTO Day (name, person_id) VALUES (@name, @person_id)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", day.Name);
            cmd.Parameters.AddWithValue("@person_id", personId);

            cmd.ExecuteNonQuery();
            // Bucle per a guardar els exercicis relacionats amb el dia a la base de dades
            foreach (var exercise in day.Exercises)
            {
                ExerciseDAO exerciseDAO = new ExerciseDAO(connection);
                exerciseDAO.SaveExercise(exercise, cmd.LastInsertedId);
            }
        }
    }


    public class ExerciseDAO
    {
        private MySqlConnection connection;

        public ExerciseDAO(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void SaveExercise(Exercise exercise, long dayId)
        {
            // Inseriex la informacio a la base de dades els exercicis 
            string query = "INSERT INTO Exercise (name, times, reps, seconds, day_id) VALUES (@name, @times, @reps, @seconds, @day_id)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@times", exercise.Times);
            cmd.Parameters.AddWithValue("@reps", exercise.Reps);
            cmd.Parameters.AddWithValue("@seconds", exercise.Seconds ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@day_id", dayId);

            cmd.ExecuteNonQuery();
        }
    }
}
