using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Answer
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public int Room_Id { get; set; }
        public string AnswerData { get; set; }
    }
    public class Answers : List<Answer>
    {

    }
}
