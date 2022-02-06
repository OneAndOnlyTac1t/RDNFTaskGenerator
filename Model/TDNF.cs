using System.Collections.Generic;
using System.Linq;

namespace TDNFGenerator.Model
{
    //!x !y !z V z !x !y V y !x z V x y z V !z x y V x !y !z
    public class TDNF
    {
        //метод, який представляє собой весь алгоритм
        public static string SimplifyDDNF(string ddnf)
        {
            string result = "";

            //розбиваємо вхідну дднф на елементантарні кон'юнкції
            string[] conjuctionsArray = ddnf.Split('V');
            List<string> conjuctionsList = new List<string>();
            foreach (var conj in conjuctionsArray)
            {
                conjuctionsList.Add(conj.Trim(' '));
            }
            //створюємо список для проведення алгоритму
            List<string> bufferResult = new List<string>(conjuctionsList);

            foreach (var element in conjuctionsList)
            {
                //пробуємо вилучити кон'юнкцію
                if (!RemoveConjuction(bufferResult, element))
                {
                    //пробуємо вилучити елемент
                    bufferResult = RemoveElement(conjuctionsList, element, bufferResult);
                }               
            }
            //формування результату
            foreach (var elem in bufferResult)
            {
                if (bufferResult.Count != 1)
                {
                    result += elem + " V ";
                }
                else
                {
                    result = elem;
                }
            }
            //вилучення зайвих символів в кінці рядку
            if (bufferResult.Count != 1)
            {
                result = result.Remove(result.Length - 3, 3);
            }
            //перевірка на те, чи було щось вилучено під час виконання алгоритму(вхід в рекурсію у випадку true)
            if (result != ddnf)
            {
                result = SimplifyDDNF(result);                   
            }
            return result;
        }              
        //метод видалення співмножника
        private static List<string> RemoveElement(List<string> input, string conjuction, List<string> resultInput)
        {
            //розбиваємо вхідну кон'юнкцію на співмножники
            List<string> elements = conjuction.Split(' ').ToList<string>();
            string result = "";
            //виділяємо певний співмножник
            foreach (var elem in elements)
            {
                //знаходимо список всіх змінних у розглядаємій кон'юнкції, які не є розглянутим співмножником
                var isBreakable = false;
                var notElem = elements.Where(k => !k.Equals(elem)).ToList();

                //розгядаємо всі кон'юнкції на вході
                foreach (var comparibleConjuction in input)
                {
                    int counter = 0;

                    //перевіряємо чи розгядаєма кон'юнкція є тою, з якої ми намагаємося вилучити співмножник
                    if (comparibleConjuction != conjuction)
                    {
                        //виділяємо всі співмножники з порівнювальної кон'юнкції
                        List<string> comparibleElements = comparibleConjuction.Split(' ').ToList<string>();
                        if (comparibleElements.Count <= elements.Count)
                        {
                            //перевіряємо чи є в порівнювальній кон'юнкції співмножник, інвертований до розглядаємого співмножника
                            if (comparibleElements.Contains('!' + elem) || comparibleElements.Contains(elem.TrimStart('!')))
                            {
                                //порівнюємо кількість співмножників, які не є розглядаємим елементом, у вхідній кон'юнкції та розглянутій кон'юнкції
                                //та дозволяємо видалити його з вхідної кон'юнкції, якщо їх кількість рівна
                                foreach (var elemInNotelem in notElem)
                                {
                                    if (comparibleElements.Contains(elemInNotelem))
                                    {
                                        counter++;
                                    }
                                }
                                if (counter == notElem.Count)
                                {
                                    isBreakable = true;
                                    break;
                                }
                            }                            
                        }
                    }

                }
                if (isBreakable)
                {
                    //формування нової кон'юнкції, без видаленого співмножника 
                    if (conjuction.EndsWith(elem))
                    {
                        result = conjuction.Replace(" " + elem, string.Empty);

                    }
                    else
                    {
                        result = conjuction.Replace(elem + " ", string.Empty);
                    }
                    break;
                }
            }

            if (result != "")
            {
                //заміна старої кон'юнкції на нову, зменшену на 1 елемент
                resultInput[resultInput.FindIndex(i => i.Equals(conjuction))] = result;
            }
            return resultInput;
        }
        //метод видалення елементарної кон'юнкції
        private static bool RemoveConjuction(List<string> input, string target)
        {
            //розглядаємо всі кон'юнкції у списку 
            foreach (var conjuction in input)
            {
                //перевіряємо чи не порівнюємо видаляєму кон'юнкцію з самою кон'юнкцією зі списку
                if (conjuction != target && target.Length >= conjuction.Length)
                {
                    //виділяємо всі елементи з видаляємої кон'юнкції
                    List<string> el1 = conjuction.Split(' ').ToList<string>();
                    int counter = 0;
                    //виділяємо всі елементи з розглядаємої кон'юнкції
                    List<string> el2 = target.Split(' ').ToList<string>();
                    //рахуємо кількість спільних елементів, якщо у видаляємій кон'юнкції їх на 1 менше, то її можна видалити зі списку
                    foreach (var element in el1)
                    {
                        if (el2.Contains(element))
                        {
                            counter++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //видалення кон'юнкції у випадку, коли це можливо
                    if (counter == el2.Count - 1 && el2.Count != el1.Count)
                    {
                        input.Remove(target);
                        return true;
                    }
                }
            }
            //кон'юнкції видалити не можна
            return false;
        }
    }
}
