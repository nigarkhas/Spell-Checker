using System;
using System.Collections.Generic;
using System.Linq;

namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please, enter your text here:");
            var input = Console.ReadLine();

            Checker.CorrectSpelling(input);
        }
    }
}
