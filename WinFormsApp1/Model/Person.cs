using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WinFormsApp1.Model
{
    public class Exercise
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Times")]
        public string Times { get; set; }

        [XmlElement(ElementName = "Reps")]
        public string Reps { get; set; }

        [XmlElement(ElementName = "seconds")]
        public int? Seconds { get; set; }
    }

    public class Day
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "exercise")]
        public List<Exercise> Exercises { get; set; }
    }

    [XmlRoot(ElementName = "person")]
    public class Person
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "surname")]
        public string Surname { get; set; }

        [XmlElement(ElementName = "age")]
        public int Age { get; set; }

        [XmlElement(ElementName = "weight")]
        public double Weight { get; set; }

        [XmlElement(ElementName = "totalDays")]
        public int TotalDays { get; set; }

        [XmlArray(ElementName = "days")]
        [XmlArrayItem(ElementName = "day")]
        public List<Day> Days { get; set; }

        [XmlIgnore]
        public List<Exercise> AllExercises
        {
            get
            {
                return Days?.SelectMany(d => d.Exercises).ToList();
            }
        }
    }
}
