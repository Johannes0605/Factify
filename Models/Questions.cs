using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [Required, StringLength(300)]
        public string QuestionText { get; set; } = string.Empty;

        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public List<Answer> Answers { get; set; } = new();
    }
}