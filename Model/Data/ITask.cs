using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model
{
    public interface ITask
    {
        string Question { get; set; }
        string CorrectAnswer { get; set; }
        string Text { get; set; }
    }
}
