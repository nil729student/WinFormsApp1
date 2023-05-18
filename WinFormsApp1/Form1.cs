using System.Xml.Serialization;
using WinFormsApp1.Model;
using Day = WinFormsApp1.Model.Day;

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
                XmlSerializer serializer = new XmlSerializer(typeof(Person));

                StreamReader reader = new StreamReader(filePath);
                _person = (Person)serializer.Deserialize(reader);
                reader.Close();

                // Populate the ComboBox with the Days
                comboBox1.DataSource = _person.Days;
                comboBox1.DisplayMember = "Name";
            }
            else
            {
                MessageBox.Show("Please select a file first");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When a day is selected in the ComboBox, show the exercises for that day
            var selectedDay = comboBox1.SelectedItem as Day;
            if (selectedDay != null)
            {
                dataGridView1.DataSource = selectedDay.Exercises;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // When a day is selected in the ComboBox, show the exercises for that day
            var selectedDay = comboBox1.SelectedItem as Day;
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
