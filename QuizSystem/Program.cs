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
            QuizzHelper.QuizIndexer<QuizzHelper.QuizItem> quiz = QuizzHelper.GetQuiz();

            foreach (QuizzHelper.QuizItem item in quiz.list)
            {
                Console.WriteLine(item.Question);
                Console.WriteLine(String.Join(',', item.Answers));

            }
        }
    }
}
