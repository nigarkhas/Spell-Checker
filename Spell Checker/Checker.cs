using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpellChecker
{
    public static class Checker
    {
        public static void CorrectSpelling(string input)
        {
            var splitter = "===";
            IEnumerable<char> inputSymbols, dictSymbols;
            var diffCount = 0;
            bool IsSingle = true, IsMultiple = true;
            var cooccurences = new List<string>();
            var result = new List<string>();

            var dictionary = input.Split(splitter)[0].Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            var text = input.Split(splitter)[1].Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in text)
            {
                inputSymbols = word.ToLower().ToCharArray();

                foreach (var phrase in dictionary)
                {
                    dictSymbols = phrase.ToLower().ToCharArray();
                    
                    diffCount = inputSymbols.MyExcept(dictSymbols).Count() + dictSymbols.MyExcept(inputSymbols).Count();

                    switch (diffCount)
                    {
                        case 0:
                            cooccurences.Add(word);
                            break;
                        case 1:
                            if (IsSingle)
                            {
                                cooccurences.Add(phrase);
                                IsMultiple = false;
                            }
                            break;
                        case 2:
                            if (IsMultiple)
                            {
                                if (Rearrange(word, phrase))
                                    cooccurences.Add(phrase); 

                                IsSingle = false;
                            }
                            break;  
                        default:
                            break;
                    }
                }

                if (!cooccurences.Any())
                    result.Add("{" + word + "?" + "}");

                else if (cooccurences.Count() == 1)
                    result.Add(cooccurences.SingleOrDefault());

                else
                    result.Add("{" + string.Join(" ", cooccurences) + "}");

                cooccurences.Clear();
            }

            Console.WriteLine(string.Join(" ", result));
        }

        public static bool Rearrange(string input, string dict)
        {
            var diffAdd = dict.ToLower().MyExcept(input.ToLower());
            var diffRemove = input.ToLower().MyExcept(dict.ToLower());

            if (diffAdd.Count() == 2 || diffRemove.Count() == 2)
            {
                if (diffRemove.Any() && (input.Contains(string.Join("", diffRemove))) 
                   || (diffAdd.Any() && dict.Contains(string.Join("", diffAdd))))
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
    }
}
