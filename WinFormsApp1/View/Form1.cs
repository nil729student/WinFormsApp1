using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WinFormsApp1.DAO;
using WinFormsApp1.Model;
using Dia = WinFormsApp1.Model.Dia;

namespace WinFormsApp1
{
    // Classe principal del formulari
    public partial class Form1 : Form
    {
        // Inicialització de l'objecte persona
        private Person _person;

        // Constructor de la classe Form1
        public Form1()
        {
            InitializeComponent();
        }

        // Acció quan es fa clic al botó1
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
                    // Obté el camí del fitxer especificat
                    textBox1.Text = openFileDialog.FileName;
                }
            }
        }
        // Acció quan es fa clic al botó2
        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    // Carrega l'arxiu XML
                    string xmlData = File.ReadAllText(filePath);

                    // Injecta una declaració DOCTYPE amb DTD
                    // Definició de la validació de l'XML
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

                    // Configura la validació de l'XML
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    settings.ValidationType = ValidationType.DTD;
                    settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                    // Crea l'objecte XmlReader
                    using (StringReader stringReader = new StringReader(xmlData))
                    using (XmlReader reader = XmlReader.Create(stringReader, settings))
                    {
                        // Carrega el document XML al DOM
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        // Si el document es carrega correctament, és vàlid
                        MessageBox.Show("El fitxer XML és vàlid.");

                        // Deserialitza l'arxiu XML perquè es pugui llegir
                        XmlSerializer serializer = new XmlSerializer(typeof(Person));
                        using (StreamReader streamReader = new StreamReader(filePath))
                        {
                            _person = (Person)serializer.Deserialize(streamReader);
                        }

                        // Guarda la persona a la base de dades
                        MySqlConnection connection = DatabaseConnection.GetConnection();
                        PersonDAO personDAO = new PersonDAO(connection);
                        personDAO.SavePerson(_person);

                        // Crida al procediment emmagatzemat
                        MySqlCommand cmd = new MySqlCommand("CALL MostExercisesDay()", connection);
                        MySqlDataReader reader2 = cmd.ExecuteReader();

                        // Obté el resultat
                        if (reader2.Read())
                        {
                            int dayId = reader2.GetInt32("day_id");
                            int exerciseCount = reader2.GetInt32("exercise_count");

                            // Mostra el resultat en un missatge
                            MessageBox.Show($"El dia {dayId} té el major nombre d'exercicis: {exerciseCount}");
                        }

                        reader2.Close();

                        // Omple el ComboBox amb els dies
                        comboBox1.DataSource = _person.Days;
                        comboBox1.DisplayMember = "Name";
                    }
                }
                catch (Exception ex)
                {
                    // Mostra el missatge d'error si l'arxiu XML no és vàlid
                    MessageBox.Show("El fitxer XML no és vàlid: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Primer has de seleccionar un fitxer xml");
            }

        }

        // Mostra qualsevol error de validació
        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }

        // Acció quan es selecciona un element al comboBox1
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Quan es selecciona un dia al ComboBox, mostra els exercicis d'aquell dia
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

        // Acció quan es fa clic al botó3
        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Person));
                StreamWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, _person);
                MessageBox.Show("Fitxer guardat");
                writer.Close();
            }
            else
            {
                MessageBox.Show("No s'ha carregat cap fitxer");
            }
        }
    }
}
