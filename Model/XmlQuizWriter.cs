using System.Collections.ObjectModel;
using System.Xml;

namespace TDNFGenerator.Model
{
    public class XmlQuizWriter
    {
        public static void CreateXmlFile(ObservableCollection<SingleTask> input, string path)
        {            
            using (XmlWriter writer = XmlWriter.Create(path))
            {               
                writer.WriteStartElement("quiz");
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
                    writer.WriteString(SingleTask.Text + task.Question);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("answer");
                    writer.WriteAttributeString("fraction", "100");
                    writer.WriteStartElement("text");
                    writer.WriteString(task.Answer);
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
