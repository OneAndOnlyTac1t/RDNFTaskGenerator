using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model.Interfaces
{
    public interface IMinimizationAlgorithm
    {
        string MinimizeFunction(string Ddnf);
    }
}
