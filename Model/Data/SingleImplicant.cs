using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model.Data
{
    public class SingleImplicant
    {
        public List<int> RowsList;
        public List<string> TruthTableRow;
        private List<string> allVariables;

        public SingleImplicant(List<string> truthTableRow, List<string> allVariablesInput)
        {
            this.TruthTableRow = truthTableRow;
            this.allVariables = allVariablesInput;
            this.RowsList = GetRowsList(truthTableRow);
        }

        private List<int> GetRowsList(List<string> truthTableRow)
        {
            List<int> affectedRows = CreateAllRows(truthTableRow.Count);
            for (int i = 0; i < affectedRows.Count; i++)
            {
                if (!andConjunction(intToRow(affectedRows[i], truthTableRow.Count), truthTableRow))
                {
                    affectedRows.RemoveAt(i);
                    i--;
                }
            }
            return affectedRows;
        }

        private List<int> CreateAllRows(int columns)
        {
            List<int> rows = new List<int>();
            for (int i = 0; i < Math.Pow(2, columns); i++)
            {
                rows.Add(i);
            }
            return rows;
        }

        private List<string> intToRow(int x, int columns)
        {
            List<string> row = new List<string>(columns);
            for (int i = columns - 1; i >= 0; i--)
            {
                row.Add(((x & (1 << i)) == 0) ? string.Concat("!", allVariables[i])  : allVariables[i]);
            }
            return row;
        }

        private bool andConjunction(List<string> a, List<string> b)
        {          
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] == b[i] || a[i] == "*" || b[i] == "*")
                {
                    continue;
                }

                return false;
            }
            return true;
        }
    }
}
