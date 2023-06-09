﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WinFormsApp1.Model
{   //classe que conté les propietats de l'exercici
    public class Exercise
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Times")]
        public int Times { get; set; }

        [XmlElement(ElementName = "Reps")]
        public int Reps { get; set; }

        [XmlElement(ElementName = "seconds")]
        public int? Seconds { get; set; }
    }
    //classe que conté les propietats del dia
    public class Dia
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "exercise")]
        public List<Exercise> Exercises { get; set; }
    }
    //classe que conté les propietats de la persona
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
        public List<Dia> Days { get; set; }

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
