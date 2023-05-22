using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Alghorithms;
using TDNFGenerator.Model.Interfaces;

namespace TDNFGenerator.Model
{
    //!t !x !y !z V !t x !y z V !t x y z V t !x y z V t x !y z V t x y !z V t x y z
    //!t !x !y z V !t !x y z V !t x !y z V !t x y z V t x y !z V t x y z
    //x y !z + !x y z + !x !y !z + x !y z + !x y !z + x y z
    //x !y !z  +  !x !y !z  +  !x y z  +  !x y !z  + x !y z
    public class QuineMcClaski : IMinimizationAlgorithm
    {

        private List<string> allVariables = new List<string>();
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
                row.Add(((x & (1 << i)) == 0) ? string.Concat("!", allVariables[i]) : allVariables[i]);
            }
            return row;
        }
        private List<string> SortConjunctions(List<string> conjuctionsList)
        {
            var allRows = CreateAllRows(allVariables.Count);
            allVariables.Reverse();
            var table = new List<List<string>>();
            for (int i = 0; i < allRows.Count; i++)
            {
                table.Add(intToRow(i, allVariables.Count));
            }
            var allConjunctionsRows = new List<List<string>>();
            for (int j = 0; j < conjuctionsList.Count; j++)
            {
                var elements = conjuctionsList[j].Split(' ').ToList();
                allConjunctionsRows.Add(elements);
            }
            SortedDictionary<int, string> indexesList = new SortedDictionary<int, string>();
            for (int i = 0; i < conjuctionsList.Count; i++)
            {
                var tables = table;
                for (int j = 0; j < allConjunctionsRows[0].Count; j++)
                {
                     tables = tables.Where(k => k.Contains(allConjunctionsRows[i][j])).ToList();
                }
                indexesList.Add(table.IndexOf(tables[0]), conjuctionsList[i]);
            }
            return indexesList.Values.ToList();
        }
        public SortedDictionary<int, Dictionary<string, bool>> GetGroups(List<string> conjuctionsList)
        {
            var maxConj = conjuctionsList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
            int numberOfGroups = maxConj.Split(' ').Length;
            SortedDictionary<int, Dictionary<string, bool>> groups = new SortedDictionary<int, Dictionary<string, bool>>();
            for (int j = 0; j < conjuctionsList.Count; j++)
            {
                var elements = conjuctionsList[j].Split(' ').ToList();
                var x = elements.Where(k => !k.Contains('!')).Count();
                if (!groups.ContainsKey(x))
                {
                    groups.Add(x, new Dictionary<string, bool>());
                }
                groups[x].Add(conjuctionsList[j], false);
            }
            return groups;
        }
        private void GetAllVariables(List<string> conjuctionsList)
        {
            if (allVariables.Count != 0)
                allVariables.Clear();
            var elems = conjuctionsList.FirstOrDefault().Split(' ').ToList();
            foreach (var elem in elems)
            {
                if (elem.StartsWith("!"))
                {
                    allVariables.Add(elem.Replace("!", string.Empty));
                }
                else
                {
                    allVariables.Add(elem);
                }
            }
        }
        public string MinimizeFunction(string ddnf)
        {
            List<string> bufferResultList = new List<string>();
            var isRunning = true;
            //розбиваємо вхідну дднф на елементантарні кон'юнкції
            string[] conjuctionsArray = ddnf.Split('+');
            List<string> conjuctionsList = new List<string>();
            foreach (var conj in conjuctionsArray)
            {
                conjuctionsList.Add(conj.Trim(' '));
            }
            GetAllVariables(conjuctionsList);
            conjuctionsList = SortConjunctions(conjuctionsList);
            do
            {
                var groups = GetGroups(conjuctionsList);
                var bufferGroups = GetGroups(conjuctionsList);
                List<string> resultList = new List<string>();
                var i = groups.Keys.Min();

                do
                {
                    if (groups.Count == 1)
                    {
                        foreach (var elem in conjuctionsList)
                        {
                            resultList.Add(elem);
                        }
                        isRunning = false;
                        break;
                    }
                    if (i != groups.Keys.Last() && groups.ContainsKey(i + 1) && groups.ContainsKey(i))
                    {
                        foreach (var conj in groups[i])
                        {
                            foreach (var comparibleConj in groups[i + 1])
                            {
                                var result = SimplifyConjunction(conj.Key, comparibleConj.Key, out var isChanged);
                                if (isChanged)
                                {
                                    bufferGroups[i][conj.Key] = true;
                                    bufferGroups[i + 1][comparibleConj.Key] = true;
                                    if (!resultList.Contains(result))
                                        resultList.Add(result);
                                }
                            }
                        }
                    }
                    i++;
                }
                while (i != groups.Keys.Last());

                if (resultList.Any())
                {
                    if (resultList.Count == 1)
                    {
                        FormatPartialResult(bufferResultList, bufferGroups);
                        isRunning = false;
                        // bufferResultList.Add(FormatConstituent(resultList.First()));
                        bufferResultList.Add(resultList.First());
                    }
                    else
                    {
                        conjuctionsList = resultList;
                        if (!FormatPartialResult(bufferResultList, bufferGroups))
                        {
                            
                        }
                    }
                }
                else
                {
                    if (bufferResultList.Count == 0)
                        bufferResultList = conjuctionsList;
                    else
                        bufferResultList.AddRange(conjuctionsList);
                    isRunning = false;
                }

            }
            while (isRunning);
            return GenerateResultString(new PetricksAlgorithm().OptimizePrimeImplicantsTable(bufferResultList, allVariables));
        }
      
        public string GenerateResultString(List<string> resultList)
        {
            var result = "";
            var last = resultList.Last();
            foreach (var elem in resultList)
            {
                result += FormatConstituent(elem);
                if (last != elem)
                {
                    result += " + ";
                }
            }
            return result;
        }
        private bool FormatPartialResult(List<string> resultList, SortedDictionary<int, Dictionary<string, bool>> table)
        {
            var minterms = (from dictionaries in table.Values
                            from value in dictionaries.Values
                            where value == false
                            select dictionaries.Where(x => x.Value == value)).ToList();
            if (minterms != null)
            {
                foreach (var el in minterms)
                {
                    foreach (var tableElement in el)
                        resultList.Add(tableElement.Key);
                }
                return true;
            }
            return false;


        }
        private string FormatConstituent(string input)
        {
            var output = input;
            if (input.StartsWith("*"))
            {
                output = output.Replace("* ", string.Empty);
            }
            output = output.Replace(" *", string.Empty);
            return output;
        }
        private string SimplifyConjunction(string conjuction, string comparibleConjuction, out bool isChanged)
        {
            List<string> conjunctionElements = conjuction.Split(' ').ToList();
            string result = null;
            isChanged = false;
            //виділяємо певний співмножник
            foreach (var elem in conjunctionElements)
            {
                //знаходимо список всіх змінних у розглядаємій кон'юнкції, які не є розглянутим співмножником
                var isBreakable = false;
                var notElemList = conjunctionElements.Where(k => !k.Equals(elem)).ToList();

                int counter = 0;

                //виділяємо всі співмножники з порівнювальної кон'юнкції
                List<string> comparibleElements = comparibleConjuction.Split(' ').ToList<string>();
                if (comparibleElements.Count <= conjunctionElements.Count)
                {
                    //перевіряємо чи є в порівнювальній кон'юнкції співмножник, інвертований до розглядаємого співмножника
                    if (comparibleElements.Contains('!' + elem) || comparibleElements.Contains(elem.TrimStart('!')))
                    {
                        //порівнюємо кількість співмножників, які не є розглядаємим елементом, у вхідній кон'юнкції та розглянутій кон'юнкції
                        //та дозволяємо видалити його з вхідної кон'юнкції, якщо їх кількість рівна
                        foreach (var elementInNotElemList in notElemList)
                        {
                            if (comparibleElements.Contains(elementInNotElemList))
                            {
                                counter++;
                            }
                        }
                        if (counter == notElemList.Count)
                        {
                            isBreakable = true;
                        }
                    }
                }
                if (isBreakable)
                {
                    //формування нової кон'юнкції, без видаленого співмножника 
                    result = conjuction.Replace(elem, "*");
                    isChanged = true;
                    break;
                }
            }
            return result;
        }
    }
}
