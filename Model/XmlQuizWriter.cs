﻿using System.Collections.ObjectModel;
using System.Xml;

namespace TDNFGenerator.Model
{
    public class XmlQuizWriter
    {
        public static void CreateXmlTestFile(ObservableCollection<ITask> input, string path)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false
            };
            using (XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings))
            {

                writer.WriteStartElement("quiz");
                writer.WriteStartElement("question");
                writer.WriteAttributeString("type", "category");
                writer.WriteStartElement("category");
                writer.WriteStartElement("text");
                writer.WriteString("$course$/top/QuineMcClasky");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                foreach (var task in input)
                {
                    var testTask = task as SingleTestTask;
                    writer.WriteStartElement("question");
                    writer.WriteAttributeString("type", "multichoice");
                    writer.WriteStartElement("name");
                    writer.WriteStartElement("text");
                    writer.WriteString("Simplify DDNf");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("questiontext");
                    writer.WriteStartElement("text");
                    writer.WriteString(task.Text + "\n" + task.Question);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    foreach (var answer in testTask.AllTestAnswers)
                    {
                        if (answer.Validity)
                        {
                            writer.WriteStartElement("answer");
                            writer.WriteAttributeString("fraction", "100");
                            writer.WriteStartElement("text");
                            writer.WriteString(answer.Content);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else
                        {
                            writer.WriteStartElement("answer");
                            writer.WriteAttributeString("fraction", "0");
                            writer.WriteStartElement("text");
                            writer.WriteString(answer.Content);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
        public static void CreateXmlFile(ObservableCollection<ITask> input, string path)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false
            };
            using (XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings))
            {
                writer.WriteStartElement("quiz");
                writer.WriteStartElement("question");
                writer.WriteAttributeString("type", "category");
                writer.WriteStartElement("category");
                writer.WriteStartElement("text");
                writer.WriteString("$course$/top/LinearMinimization");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                foreach (var task in input)
                {
                    writer.WriteStartElement("question");
                    writer.WriteAttributeString("type", "shortanswer");

                    writer.WriteStartElement("name");
                    writer.WriteStartElement("text");
                    writer.WriteString("Simplify DDNf");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("questiontext");
                    writer.WriteStartElement("text");
                    writer.WriteString(task.Text + task.Question);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("answer");
                    writer.WriteAttributeString("fraction", "100");
                    writer.WriteStartElement("text");
                    writer.WriteString(task.CorrectAnswer);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
