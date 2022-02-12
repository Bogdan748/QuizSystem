using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;



namespace QuizSystem
{
    public class QuizzHelper
    {
        public class QuizIndexer<QuizItem>
        {
            //Using an Indexer of type List
            private List<QuizItem> list = new List<QuizItem>();
            public double mark =0;

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
            //A class that stores the information for each Item = Question, Options, Corect Responses
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


        public static QuizIndexer<QuizItem> GetQuiz()
        {
            //A metod to start the Quiz

            QuizIndexer<QuizItem> Quiz = new QuizIndexer<QuizItem>();

            //Gets the cel values of a CSV,and with the help of QuizItem Constructor, makes an object for each row of the CSV
            using (TextFieldParser parser = new TextFieldParser(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Quiz.csv")))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                
                bool firstLine = true;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (firstLine)//ignores header
                    {
                        firstLine = false;
                        continue;
                    }
                    
                    string question = fields[0];
                    int[] correct = fields[1].Split(';').Select(Int32.Parse).ToArray();
                    string[] answers = fields.Skip(2).ToArray();

                    //QuizItem Constructor
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
                //Checks if corect and scores the Quiz
                quiz.mark+=Response(correct);
                Console.WriteLine($"---------------------------");
            }

            Console.WriteLine($"Quiz ended. Your final mark is {quiz.mark} out of {quiz.listLength()}");
        }

        public static double Response(bool[] array)
        {
            Console.WriteLine("");
            Console.Write("FOR TESTING PURPOSES: Corect Items: ");
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j]) Console.Write($"{j + 1} | ");
            }
            Console.WriteLine("");

            double score = 0;
            
            int nrOfOptions = 0;

            foreach (bool corect in array)
            {
                if (corect) nrOfOptions++;
            }

            double scorePerOption = Math.Round((double)1 / (double)nrOfOptions, 2);
            Console.WriteLine();
            Console.WriteLine("Type correct option/options is/are: (The format should be '1,5,2')");
            Console.Write("Answer:  ");
            string input = Console.ReadLine();

            int[] responses = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            
            //The marking rule is: you get a partial score for each right answer, but if you have a wrong answer the total score is 0 
            foreach(int resp in responses)
            {
                if (resp > array.Length || resp<1)
                {
                    score = 0;
                    break;
                } 

                if (array[resp - 1])
                {
                    score += scorePerOption;
                }
                else
                {
                    score = 0;
                    break;
                }
            }

            if (1d - score < 0.1d)
            {
                return 1;
            }else
            {
                return score;
            }
            
        }

    }
}
