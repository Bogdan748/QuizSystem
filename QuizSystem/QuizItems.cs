using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizSystem
{
    public class QuizIndexer
    {
        private List<QuizItem> list = new List<QuizItem>();
        
        public QuizIndexer(QuizItem item)
        {
            this.list.Add(item);
        }

        public QuizItem this[int index]
        {
            get { return this.list[index]; }
            set { this.list[index] = value; }
        }
    }

    public class QuizItem
    {
        private string Question;
        private Dictionary<string, bool> Answers= new Dictionary<string, bool>();
        

        public QuizItem(string question, string[] answers, bool[] correct )
        {
            this.Question = question;
            
            for (int i=0; i< answers.Length; i++)
            {
                this.Answers.Add(answers[i], correct[i]);
            }

        }
    }
}
