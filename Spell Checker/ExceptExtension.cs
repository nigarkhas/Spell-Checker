using System;
using System.Collections.Generic;
using System.Text;

namespace SpellChecker
{
    public static class ExceptExtension
    {
        public static IEnumerable<TSource> MyExcept<TSource>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second)
        {
            return NonDistinctExcept(first, second);
        }

        public static IEnumerable<TSource> NonDistinctExcept<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            var secondCounts = new Dictionary<TSource, int>();
            int place;

            foreach (var letter in second)
            {

                if (secondCounts.TryGetValue(letter, out place))
                {
                    secondCounts[letter] = place + 1;
                }
                else
                {
                    secondCounts.Add(letter, 1);
                }

            }

            foreach (var letter in first)
            {

                if (secondCounts.TryGetValue(letter, out place))
                {
                    if (place == 0)
                    {
                        secondCounts.Remove(letter);
                        yield return letter;
                    }
                    else
                    {
                        secondCounts[letter] = place - 1;
                    }
                }
                else
                {
                    yield return letter;
                }

            }
        }
    }
}
