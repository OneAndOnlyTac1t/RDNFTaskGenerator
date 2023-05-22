using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Interfaces;
using TDNFGenerator.Model.Data;
using TDNFGenerator.Model.Alghorithms;

namespace TDNFGenerator.Model
{
    class WrongAnswersGenerator
    {
        private string CorrectAnswer { get; set; }

        private AlghorithmsContext SelectedAlgorithm;
        private List<TestOption> WrongAnswers { get; set; } = new List<TestOption>();
        private List<string> Variables { get; set; } = new List<string>();
        public List<TestOption> Answers { get; set; } = new List<TestOption>();
        Random _random = new Random();
        private string InputList { get; set; }

        public WrongAnswersGenerator(string correctAnswer, string input, AlghorithmsContext algo, int wrongAnswersConunt = 3)
        {
            SelectedAlgorithm = algo;
            CorrectAnswer = correctAnswer;
            InputList = input;
            GetVariablesFromInput();
            for (int i = 0; i < wrongAnswersConunt; i++)
            {
                var newDDNF = ModifyQuestion();
                AddNewWrongAnswer(newDDNF);              
            }
            Answers = WrongAnswers;
            var index = _random.Next(0, Answers.Count);
            Answers.Insert(index, new TestOption() { Content = CorrectAnswer, Validity = true });
        }

        private void AddNewWrongAnswer(string newDDNF)
        {
            string result = SelectedAlgorithm.ExecuteMinimization(newDDNF);           
            if (WrongAnswers.Where(l=>l.Content==result).Any())
            {
                AddNewWrongAnswer(ModifyQuestion());
            }
            else
            {
                WrongAnswers.Add(new TestOption() { Content = result, Validity = false });
                return;
            }
        }
        private string ModifyQuestion()
        {
            var inputConstituents = InputList.Split('+').ToList();
            inputConstituents.ForEach(l => l.TrimStart(' '));
            inputConstituents.ForEach(l => l.TrimEnd(' '));
            List<List<string>> comparableList;
            int index;
            List<string> tempSelectedVariablesList;
            do
            {
                var tempResult = new List<string>(inputConstituents);
                index = _random.Next(0, inputConstituents.Count);
                var tempSelectedConstituent = inputConstituents[index];
                tempSelectedVariablesList = new List<string>();
                foreach (var tempElem in tempSelectedConstituent.Split(' '))
                {
                    if (tempElem != string.Empty)
                        tempSelectedVariablesList.Add(tempElem);
                }
                ChangeSingleVariable(tempSelectedVariablesList);
                comparableList = GetAllList(inputConstituents);
            } 
            while (!CheckValidity(tempSelectedVariablesList, comparableList));
            string resultConstituent="";
            foreach(var elem in tempSelectedVariablesList)
            {
                if (tempSelectedVariablesList.Last() != elem)
                {
                    resultConstituent += elem + " ";
                }
                else
                {
                    resultConstituent += elem;
                }
            }
            inputConstituents.RemoveAt(index);
            inputConstituents.Insert(index, resultConstituent);
            string result="";
            //формування результату
            return (new QuineMcClaski()).GenerateResultString(inputConstituents);
        }

        private bool CheckValidity(List<string> tempSelectedVariablesList, List<List<string>> allList)
        {
            var result = true;
            int counter = 0;

            foreach (var list in allList)
            {
                foreach (var tempvar in tempSelectedVariablesList)
                {
                    if (list.Contains(tempvar))
                    {
                        counter++;
                    }
                }
                if (counter == tempSelectedVariablesList.Count)
                {
                    return false;
                }
                else
                {
                    counter = 0;
                }
            }
            return result;
        }
        private List<List<string>> GetAllList(List<string> tempConstituents)
        {
            var result = new List<List<string>>();
            foreach (var constiuent in tempConstituents)
            {
                var newList = new List<string>();
                foreach (var elem in constiuent.Split(' '))
                {
                    if (elem != string.Empty)
                        newList.Add(elem);
                }
                result.Add(newList);
            }
            return result;
        }
        private void ChangeSingleVariable(List<string> variables)
        {
            var randIndex = _random.Next(0, variables.Count);
            var tempVar = variables[randIndex];
            if (tempVar.Contains('!'))
            {
                tempVar = tempVar.Replace("!", string.Empty);
            }
            else
            {
                tempVar = "!" + tempVar;
            }
            variables.RemoveAt(randIndex);
            variables.Insert(randIndex, tempVar);

        }
        private List<string> GetVariablesFromConstituent(string constituent)
        {
            var result = new List<string>();
            foreach (var tempElem in constituent.Split(' '))
            {
                if (tempElem.StartsWith("!"))
                {
                    result.Add(tempElem.TrimStart('!'));
                }
                else
                {
                    if (tempElem != string.Empty)
                        result.Add(tempElem);
                }
            }
            return result;
        }
        private void GetVariablesFromInput()
        {

            var charArr = InputList.Split('+');
            foreach (string elem in charArr)
            {
                Variables = GetVariablesFromConstituent(elem);
                return;
            }
        }
    }
}
