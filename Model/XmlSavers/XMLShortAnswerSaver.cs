using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TDNFGenerator.Model.Interfaces;

namespace TDNFGenerator.Model.XmlSavers
{
    public class XMLShortAnswerSaver : IXmlSaveMethod
    {
        public void SaveXml(ObservableCollection<ITask> input, IMinimizationAlgorithm minimizationAlgorithm, string path)
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
                if (minimizationAlgorithm is TDNF)
                    writer.WriteString("$course$/top/LinearMinimization");
                else
                    writer.WriteString("$course$/top/QuineMcClasky");
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
