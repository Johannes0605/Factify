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

        public async Task<IActionResult> Start(int id)
        {
            var quiz = await _repository.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();

            return View(quiz);
        }

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
                var correctAnswers = question.Options
                    .Where(a => a.IsCorrect)
                    .Select(a => a.OptionsId)
                    .ToList();

                var chosen = selectedAnswers
                    .Where(id => question.Options.Any(a => a.OptionsId == id))
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
