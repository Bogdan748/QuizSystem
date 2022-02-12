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
            public void Add(QuizItem item)
                {
                list.Add(item);
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



    }
}
