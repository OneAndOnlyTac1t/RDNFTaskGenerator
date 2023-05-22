using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Interfaces;

namespace TDNFGenerator.Model.Alghorithms
{
    public class AlghorithmsContext
    {
        public IMinimizationAlgorithm chosenAlghorithm { get; private set; }

        public AlghorithmsContext(IMinimizationAlgorithm minimization)
        {
            chosenAlghorithm = minimization;
        }
        public string ExecuteMinimization(string DDNF)
        {
            return chosenAlghorithm.MinimizeFunction(DDNF);
        }
        public void SetAlgorithm(IMinimizationAlgorithm minimizationAlgorithm)
        {
            chosenAlghorithm = minimizationAlgorithm;
        }
    }
}
