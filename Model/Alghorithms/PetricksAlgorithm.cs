using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Data;

namespace TDNFGenerator.Model.Alghorithms
{
    public class PetricksAlgorithm
    {
        public List<string> OptimizePrimeImplicantsTable(List<string> implicants, List<string> allVariables)
        {
            if (implicants.Count == 0)
            {
                return null;
            }

            var table = CreateTableFromIplicantsList(implicants);
            var rotatedTable = RotateTable(table);
            List<SingleImplicant> primeImplicantsTable = new List<SingleImplicant>();
            for (int i = 0; i < table.Count; i++)
            {
                primeImplicantsTable.Add(new SingleImplicant(table[i], allVariables));
            }

            List<List<SingleImplicant>> chartEquationAndConnected = GetChartEquation((int)Math.Pow(2, table[0].Count), primeImplicantsTable);

            List<SingleImplicant> requiredPrimeImplicants = GetNeccessaryPrimeImplicants(chartEquationAndConnected);

            List<List<string>> result = new List<List<string>>();
            for (int i = 0; i < requiredPrimeImplicants.Count; i++)
            {
                result.Add(requiredPrimeImplicants[i].TruthTableRow);
            }


            return FormatToResult(result);
        }

        private static List<string> FormatToResult(List<List<string>> resultTable)
        {
            var resultList = new List<string>();
            foreach(var elem in resultTable)
            {
                string conjuction = "";
                foreach(var variable in elem)
                {
                    if (variable != "*")
                    {
                        if (conjuction == "")
                            conjuction += variable;
                        else
                            conjuction += " " + variable;
                    }
                }
                resultList.Add(conjuction);
            }
            return resultList;
        }
        private static List<List<SingleImplicant>> GetChartEquation(int rows, List<SingleImplicant> primeImplicantChart)
        {
            List<List<SingleImplicant>> result = new List<List<SingleImplicant>>();
            for (int i = 0; i < rows; i++)
            {
                result.Add(GetPrimeImplicantsRow(i, primeImplicantChart));
            }
            return result;
        }

        private static List<SingleImplicant> GetPrimeImplicantsRow(int row, List<SingleImplicant> primeImplicantChart)
        {
            List<SingleImplicant> result = new List<SingleImplicant>();
            for (int i = 0; i < primeImplicantChart.Count; i++)
            {
                if (primeImplicantChart[i].RowsList.Contains(row))
                {
                    result.Add(primeImplicantChart[i]);
                }
            }
            return result;
        }

        // takes an equation like (K+L)(K+M)(L+N)(M+P)(N+Q)(P+Q)
        // expands the components to KKLMNP+KKLMNQ+... and returns the shortest
        private static List<SingleImplicant> GetNeccessaryPrimeImplicants(List<List<SingleImplicant>> andConnectedEquation)
        {
            List<List<SingleImplicant>> expanded = CalculateSum(andConnectedEquation);
            if (expanded.Count == 0)
            {
                return null;
            }
            List<SingleImplicant> shortest = expanded[0];
            foreach (List<SingleImplicant> term in expanded)
            {
                // remove duplicates
                for (int i = 0; i < term.Count; i++)
                {
                    while (term.LastIndexOf(term[i]) != i)
                    {
                        term.RemoveAt(term.LastIndexOf(term[i]));
                    }
                }

                if (shortest.Count > term.Count)
                {
                    shortest = term;
                }
            }
            return shortest;
        }

        public static List<List<SingleImplicant>> CloneObject(List<List<SingleImplicant>> list)
        {
            List<List<SingleImplicant>> clone = new List<List<SingleImplicant>>();
            for (int i = 0; i < list.Count; i++)
            {
                clone.Add(list[i]);
            }

            return clone;
        }
        private static List<List<SingleImplicant>> CalculateSum(List<List<SingleImplicant>> b)
        {
            List<List<SingleImplicant>> result = new List<List<SingleImplicant>>();
            if (b.Count <= 1)
            {
                for (int i = 0; i < b[0].Count; i++)
                {
                    result.Add(new List<SingleImplicant>() { b[0][i] });
                }
            }
            else
            {
                List<SingleImplicant> head = b[0];
                List<List<SingleImplicant>> body = b;
                body.Remove(head);

                List<List<SingleImplicant>> bodyExpanded = CalculateSum(CloneObject(body));

                if (head.Count == 0)
                {
                    return bodyExpanded;
                }

                for (int i = 0; i < head.Count; i++)
                {
                    if (bodyExpanded.Count == 0)
                    {
                        List<SingleImplicant> term = new List<SingleImplicant>();
                        term.Add(head[i]);
                        result.Add(term);
                    }
                    else
                    {
                        for (int j = 0; j < bodyExpanded.Count; j++)
                        {
                            List<SingleImplicant> term = new List<SingleImplicant>();
                            term.Add(head[i]);
                            term.AddRange(bodyExpanded[j]);
                            result.Add(term);
                        }
                    }
                }
            }
            return result;
        }
        private static List<List<string>> CreateTableFromIplicantsList(List<string> implicants)
        {
            List<List<string>> table = new List<List<string>>();
            for(int i=0; i<implicants.Count; i++)
            {
                var array = implicants[i].Split(' ');
                table.Add(new List<string>());
                foreach(var elem in array)
                {
                    table[i].Add(elem);
                }
            }
            return table;
        }
        private static List<List<string>> RotateTable(List<List<string>> table)
        {
            List<List<string>> transposed = new List<List<string>>();

            if (table.Count == 0)
            {
                return table;
            }

            for (int i = 0; i < table[0].Count; i++)
            {
                transposed.Add(new List<string>());
                for (int j = 0; j < table.Count; j++)
                {
                    transposed[i].Add(table[j][i]);
                }
            }

            return transposed;
        }
    }
}
