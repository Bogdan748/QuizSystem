using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizSystem
{
    public class QuizzHelper
    {
        public class QuizIndexer<QuizItem>
        {
            public List<QuizItem> list = new List<QuizItem>();
            public decimal mark=0;

            public void Add(QuizItem item)
                {
                list.Add(item);
                }
            public QuizItem this[int index]
            {
                get => list[index];
            }

            public int listLength()
            {
                return this.list.Count;
            }
        }

        public class QuizItem
        {
            public string Question;
            public Dictionary<string, bool> Answers = new Dictionary<string, bool>();


            public QuizItem(string question, string[] answers, int[] correct)
            {
                this.Question = question;

                for (int i = 0; i < answers.Length; i++)
                {
                    if(!String.IsNullOrEmpty(answers[i])) this.Answers.Add(answers[i], correct.Contains(i+1));
                }

            }
        }


        public static QuizzHelper.QuizIndexer<QuizItem> GetQuiz()
        {
            QuizzHelper.QuizIndexer<QuizItem> Quiz = new QuizIndexer<QuizItem>();
            
            using (TextFieldParser parser = new TextFieldParser("D:\\Fast Track IT\\Github4\\QuizSystem\\QuizSystem\\Quiz.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                
                bool firstLine = true;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (firstLine)//ignor firs row
                    {
                        firstLine = false;
                        continue;
                    }
                    //Process row
                    string question = fields[0];
                    int[] correct = fields[1].Split(';').Select(Int32.Parse).ToArray();
                    string[] answers = fields.Skip(2).ToArray();

                    Quiz.Add(new QuizItem(question, answers, correct));
                }
            }
            return Quiz;
        }


        public static void StartQuiz()
        {
            //Initializing the quiz
            QuizIndexer<QuizItem> quiz = GetQuiz();

            //Initializat a list of numbers reprezenting the items indexed in quiz
            List<int> questions = Enumerable.Range(0, quiz.listLength()).ToList();

            Random rnd = new Random();

            for (int i=0; i< quiz.listLength(); i++)
            {
               //Get a random index of the list of items 
                int currentItemIndex = rnd.Next(questions.Count);
                

                Console.WriteLine($"Question {i+1}: {quiz[questions[currentItemIndex]].Question}{Environment.NewLine}");
                Console.WriteLine($"Options:");

                //Init a list of option of the current question
                int optionCount = quiz[questions[currentItemIndex]].Answers.Count;
                List<int> options = Enumerable.Range(0, optionCount ).ToList();

                //Initlialize array of correct items
                bool[] correct = new bool[optionCount];


                for(int j=0; j < optionCount; j++)
                {
                    //Get a random index of the list of options
                    int currentOptionIndex = rnd.Next(options.Count);

                    Console.WriteLine($"{j+1}. {quiz[questions[currentItemIndex]].Answers.ElementAt(options[currentOptionIndex]).Key}");

                    //Mark the correct and incorect options
                    correct[j] = quiz[questions[currentItemIndex]].Answers.ElementAt(options[currentOptionIndex]).Value;

                    //Remove index of option
                    options.RemoveAt(currentOptionIndex);

                }

                //Eliminate the item from the list
                questions.RemoveAt(currentItemIndex);
                Console.WriteLine($"---------------------------");
                //TODO: CHECK IF CORRECT
                quiz.mark+=Response(correct);
                
                Console.WriteLine($"---------------------------");
            }

            Console.WriteLine($"Your mark is {quiz.mark}");
        }

        public static decimal Response(bool[] array)
        {
            Console.WriteLine("");
            Console.Write("Corect Items:");
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j]) Console.Write(j+1);
            }
            Console.WriteLine("");

            decimal score = 0;
            /*
            int nrOfOptions = 0;

            foreach (bool corect in array)
            {
                if (corect) nrOfOptions++;
            }
            
            decimal scorePerOption = 1 / nrOfOptions;
            */
            Console.WriteLine("Type correct option/options is/are: (The format should be '1,5,2')");

            string input = Console.ReadLine();

            int[] responses = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            foreach(int resp in responses)
            {
                if (array[resp - 1]) score++;//= scorePerOption;
            }

            return score;
        }

    }
}
