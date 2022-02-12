using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using QuizSystem;

namespace QuizSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            QuizzHelper.StartQuiz();

            Console.WriteLine("");
            Console.WriteLine("Redo the quiz (y/n)?");
            string input = Console.ReadLine();
            if (input == "y")
            {
                Console.WriteLine($"---------------------------{Environment.NewLine}");
                QuizzHelper.StartQuiz();
            }
            
        }
    }
}
