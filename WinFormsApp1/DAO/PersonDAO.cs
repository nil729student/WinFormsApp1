using System;
using System.Data;
using MySql.Data.MySqlClient;
using WinFormsApp1.Model;

namespace WinFormsApp1.DAO
{
    public class PersonDAO
    {
        private MySqlConnection connection;

        public PersonDAO(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void SavePerson(Person person)
        {
            try
            {
                string query = "INSERT INTO Person (name, surname, age, weight, totalDays) VALUES (@name, @surname, @age, @weight, @totalDays)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", person.Name);
                cmd.Parameters.AddWithValue("@surname", person.Surname);
                cmd.Parameters.AddWithValue("@age", person.Age);
                cmd.Parameters.AddWithValue("@weight", person.Weight);
                cmd.Parameters.AddWithValue("@totalDays", person.TotalDays);

                cmd.ExecuteNonQuery();

                foreach (var day in person.Days)
                {
                    DayDAO dayDAO = new DayDAO(connection);
                    dayDAO.SaveDay(day, cmd.LastInsertedId);
                }
                MessageBox.Show("PERSONA inserida correctament");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hi ha agut un error al insertar la persona: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
    }

    public class DayDAO
    {
        private MySqlConnection connection;

        public DayDAO(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void SaveDay(Dia day, long personId)
        {
            try
            {
                string query = "INSERT INTO Day (name, person_id) VALUES (@name, @person_id)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", day.Name);
                cmd.Parameters.AddWithValue("@person_id", personId);

                cmd.ExecuteNonQuery();

                foreach (var exercise in day.Exercises)
                {
                    ExerciseDAO exerciseDAO = new ExerciseDAO(connection);
                    exerciseDAO.SaveExercise(exercise, cmd.LastInsertedId);
                }
                MessageBox.Show("DIA inserit correctament");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el dia: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}
