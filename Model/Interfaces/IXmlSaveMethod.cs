using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model.Interfaces
{
    public interface IXmlSaveMethod
    {
        void SaveXml(ObservableCollection<ITask> input, IMinimizationAlgorithm minimizationAlgorithm, string path);
    }
}
