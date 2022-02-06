using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDNFGenerator.Model
{
    public class QuineMcClaski
    {
        public static List<List<string>> GetGroups(List<string> conjuctionsList)
        {
            var maxConj = conjuctionsList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
            int numberOfGroups = maxConj.Split(' ').Length;
            List<List<string>> groups = new List<List<string>>();
            for (int i = 0; i < numberOfGroups; i++)
            {
                groups.Add(new List<string>());
            }
            for (int j = 0; j < conjuctionsList.Count; j++)
            {
                var elements = conjuctionsList[j].Split(' ').ToList();
                var x = elements.Where(k => !k.Contains('!')).Count();
                groups[x].Add(conjuctionsList[j]);
            }
            return groups;
        }
        public static string QuineMcClaskiAlgorithm(string ddnf)
        {
            string result = "";

            //розбиваємо вхідну дднф на елементантарні кон'юнкції
            string[] conjuctionsArray = ddnf.Split('V');
            List<string> conjuctionsList = new List<string>();
            foreach (var conj in conjuctionsArray)
            {
                conjuctionsList.Add(conj.Trim(' '));
            }
            var groups = GetGroups(conjuctionsList);
            for(int i = 0; i<groups.Count; i++)
            {
                foreach (var conj in groups[i])
                {
                    if (i != 0||i!=groups.Count-1)
                    {
                        
                    }
                }
            }
            List<string> bufferResult = new List<string>(conjuctionsList);
            return null;
        }

        private static void SimplifyConjunction(string conjunction, List<string> group)
        {
            List<string> conjunctionElements = conjunction.Split(' ').ToList<string>();

        }
    }   
}
