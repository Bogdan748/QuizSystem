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
            using (TextFieldParser parser = new TextFieldParser("D:\\Fast Track IT\\Github4\\QuizSystem\\QuizSystem\\Quiz.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int itemCount = 0;
                while (!parser.EndOfData)
                {
                    
                    bool firstLine = true;
                    //Process row
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        if (firstLine)//ignora primul rand
                        {
                            firstLine = false;
                            continue;
                        }

                        QuizItems items = new QuizItems();
                        //TODO: Process field
                        itemCount += 1;
                    }
                }
            }
        }
    }
}
