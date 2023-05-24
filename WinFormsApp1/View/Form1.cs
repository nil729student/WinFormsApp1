using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WinFormsApp1.DAO;
using WinFormsApp1.Model;
using Dia = WinFormsApp1.Model.Dia;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Person _person;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    textBox1.Text = openFileDialog.FileName;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    // Load the XML file
                    string xmlData = File.ReadAllText(filePath);

                    // Inject a DOCTYPE declaration with DTD
                    string doctype = "<!DOCTYPE person [" +
                        "<!ELEMENT person (name, surname, age, weight, totalDays, days)>" +
                        "<!ELEMENT name (#PCDATA)>" +
                        "<!ELEMENT surname (#PCDATA)>" +
                        "<!ELEMENT age (#PCDATA)>" +
                        "<!ELEMENT weight (#PCDATA)>" +
                        "<!ELEMENT totalDays (#PCDATA)>" +
                        "<!ELEMENT days (day*)>" +
                        "<!ELEMENT day (exercise*)>" +
                        "<!ATTLIST day name CDATA #REQUIRED>" +
                        "<!ELEMENT exercise (Name, Times, Reps?, seconds)>" +
                        "<!ELEMENT Name (#PCDATA)>" +
                        "<!ELEMENT Times (#PCDATA)>" +
                        "<!ELEMENT Reps (#PCDATA)>" +
                        "<!ELEMENT seconds (#PCDATA)>" +
                        "]>";
                    int insertIndex = xmlData.IndexOf('>') + 1;
                    xmlData = xmlData.Insert(insertIndex, doctype);

                    // Set the validation settings
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    settings.ValidationType = ValidationType.DTD;
                    settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                    // Create the XmlReader object
                    using (StringReader stringReader = new StringReader(xmlData))
                    using (XmlReader reader = XmlReader.Create(stringReader, settings))
                    {
                        // Load the XML document into the DOM
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        // If the document was loaded successfully, it's valid
                        MessageBox.Show("XML file is valid.");

                        // Deserialize the XML file
                        XmlSerializer serializer = new XmlSerializer(typeof(Person));
                        using (StreamReader streamReader = new StreamReader(filePath))
                        {
                            _person = (Person)serializer.Deserialize(streamReader);
                        }

                        // Save the Person to the database
                        MySqlConnection connection = DatabaseConnection.GetConnection();
                        PersonDAO personDAO = new PersonDAO(connection);
                        personDAO.SavePerson(_person);

                        // Call the stored procedure
                        MySqlCommand cmd = new MySqlCommand("CALL MostExercisesDay()", connection);
                        MySqlDataReader reader2 = cmd.ExecuteReader();

                        // Get the result
                        if (reader2.Read())
                        {
                            int dayId = reader2.GetInt32("day_id");
                            int exerciseCount = reader2.GetInt32("exercise_count");

                            // Show the result in a message box
                            MessageBox.Show($"Day {dayId} has the most number of exercises: {exerciseCount}");
                        }

                        reader2.Close();

                        // Populate the ComboBox with the Days
                        comboBox1.DataSource = _person.Days;
                        comboBox1.DisplayMember = "Name";
                    }
                }
                catch (Exception ex)
                {
                    // Show the error message if the XML file is not valid
                    MessageBox.Show("XML file is not valid: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Primer has de seleccionar un fitxer xml");
            }

        }

        // Display any validation errors
        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When a day is selected in the ComboBox, show the exercises for that day
            var selectedDay = comboBox1.SelectedItem as Dia;
            if (selectedDay != null)
            {
                dataGridView1.DataSource = selectedDay.Exercises;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Quan seleccioni el dia mostri els dies associats
            var selectedDay = comboBox1.SelectedItem as Dia;
            if (selectedDay != null)
            {
                dataGridView1.DataSource = selectedDay.Exercises;
                dataGridView1.Columns["Name"].Width = 200;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Person));
                StreamWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, _person);
                MessageBox.Show("File Saved");
                writer.Close();
            }
            else
            {
                MessageBox.Show("No file loaded");
            }
        }

      
    }
}
