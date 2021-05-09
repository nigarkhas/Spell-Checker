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
            var diffCount = 0;
            bool IsSingle = true, IsMultiple = true;
            var cooccurences = new List<string>();
            var result = new List<string>();

            try
            {
                var dictionary = input.Split(splitter)[0].Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                var text = input.Split(splitter)[1].Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);


                foreach (var word in text)
                {
                    IsSingle = !(dictionary.Where(x => EditDistance(word.ToLower(), x.ToLower(), word.Length, x.Length) == 0)).Any();
                    IsMultiple = !(dictionary.Where(x => EditDistance(word.ToLower(), x.ToLower(), word.Length, x.Length) == 1)).Any();

                    foreach (var phrase in dictionary)
                    {
                        diffCount = EditDistance(word.ToLower(), phrase.ToLower(), word.Length, phrase.Length);

                        switch (diffCount)
                        {
                            case 0:
                                cooccurences.Add(phrase);
                                IsSingle = false; IsMultiple = false;
                                break;
                            case 1:
                                if (IsSingle)
                                    cooccurences.Add(phrase);
                                break;
                            case 2:
                                if (IsMultiple && Rearrange(word, phrase))
                                    cooccurences.Add(phrase);
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
            catch 
            {
                Console.WriteLine("Input is incorrect");
            }
        }

        public static bool Rearrange(string input, string dict)
        {
            var diffAdd = dict.ToLower().MyExcept(input.ToLower());
            var diffRemove = input.ToLower().MyExcept(dict.ToLower());

            if ((diffAdd.Count() == 2 || diffRemove.Count() == 2))
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

        public static int EditDistance(string input, string dict, int lenIn, int lenDi)
        {
            if (lenIn == 0)
                return lenDi;

            if (lenDi == 0)
                return lenIn;

            if (input[lenIn - 1] == dict[lenDi - 1])
                return EditDistance(input, dict, lenIn - 1, lenDi - 1);

            return 1 + Math.Min(EditDistance(input, dict, lenIn, lenDi - 1), EditDistance(input, dict, lenIn - 1, lenDi));
        }

    }
}
