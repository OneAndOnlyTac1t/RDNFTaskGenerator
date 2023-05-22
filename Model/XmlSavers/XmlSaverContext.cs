using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Interfaces;

namespace TDNFGenerator.Model.XmlSavers
{
    public class XmlSaverContext
    {
        private IXmlSaveMethod xmlSaveMethod;

        public XmlSaverContext(IXmlSaveMethod saveMethod)
        {
            xmlSaveMethod = saveMethod;
        }
        public void ExecuteXmlSaveMethod(ObservableCollection<ITask> input, Alghorithms.AlghorithmsContext minimizationAlgorithm, string path)
        {
            xmlSaveMethod.SaveXml(input, minimizationAlgorithm.chosenAlghorithm, path);
        }
        public void SetXmlSaveMethod(IXmlSaveMethod saveMethod)
        {
            xmlSaveMethod = saveMethod;
        }
    }
}
