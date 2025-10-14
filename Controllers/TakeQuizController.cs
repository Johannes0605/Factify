using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;

namespace QuizApp.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly IQuizRepository _repository;
        private readonly ILogger<TakeQuizController> _logger;

        public TakeQuizController(IQuizRepository repository, ILogger<TakeQuizController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: /QuizPlay/Start/5
        public async Task<IActionResult> Start(int id)
        {
            var quiz = await _repository.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // POST: /QuizPlay/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(int quizId, List<int> selectedAnswers)
        {
            var quiz = await _repository.GetQuizByIdAsync(quizId);
            if (quiz == null) return NotFound();

            int score = 0;
            int total = 0;

            foreach (var question in quiz.Questions)
            {
                total++;
                var correctAnswers = question.Answers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.AnswerId)
                    .ToList();

                var chosen = selectedAnswers
                    .Where(id => question.Answers.Any(a => a.AnswerId == id))
                    .ToList();

                if (chosen.SequenceEqual(correctAnswers))
                    score++;
            }

            ViewBag.Score = score;
            ViewBag.Total = total;
            return View("Result");
        }
    }
}
