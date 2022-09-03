using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model
{
    //!t !x !y !z V !t x !y z V !t x y z V t !x y z V t x !y z V t x y !z V t x y z
    //!t !x !y z V !t !x y z V !t x !y z V !t x y z V t x y !z V t x y z
    public class QuineMcClaski
    {
        public static SortedDictionary<int, Dictionary<string, bool>> GetGroups(List<string> conjuctionsList)
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
        public static string QuineMcClaskiAlgorithm(string ddnf)
        {
            List<string> bufferResultList = new List<string>();
            var isRunning = true;
            //розбиваємо вхідну дднф на елементантарні кон'юнкції
            string[] conjuctionsArray = ddnf.Split('V');
            List<string> conjuctionsList = new List<string>();
            foreach (var conj in conjuctionsArray)
            {
                conjuctionsList.Add(conj.Trim(' '));
            }
            do
            {
                var groups = GetGroups(conjuctionsList);
                var bufferGroups = GetGroups(conjuctionsList);
                List<string> resultList = new List<string>();
                var i = groups.Keys.Min();

                do
                {
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
                        bufferResultList.Add(FormatConstituent(resultList.First()));
                    }
                    else
                    {
                        conjuctionsList = resultList;
                        FormatPartialResult(bufferResultList, bufferGroups);
                    }
                }
                else
                {
                    isRunning = false;
                }

            }
            while (isRunning);
            return GenerateResultString(bufferResultList);
        }

        private static string GenerateResultString(List<string> resultList)
        {
            var result = "";
            var last = resultList.Last();
            foreach (var elem in resultList)
            {
                result += FormatConstituent(elem);
                if (last != elem)
                {
                    result += " V ";
                }
            }
            return result;
        }
        private static void FormatPartialResult(List<string> resultList, SortedDictionary<int, Dictionary<string, bool>> table)
        {
            var minterms = (from dictionaries in table.Values
                            from value in dictionaries.Values
                            where value == false
                            select dictionaries.Where(x => x.Value == value)).FirstOrDefault();

            foreach (var el in minterms)
            {
                resultList.Add(el.Key);
            }

        }
        private static string FormatConstituent(string input)
        {
            var output= input;
            if (input.StartsWith("*"))
            {
                output = output.Replace("* ", string.Empty);
            }
            output = output.Replace(" *", string.Empty);
            return output;
        }
        private static string SimplifyConjunction(string conjuction, string comparibleConjuction, out bool isChanged)
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
