using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _repository;
        private readonly ILogger<QuizController> _logger;

        // ✅ Bruk repository og logger i stedet for DbContext
        public QuizController(IQuizRepository repository, ILogger<QuizController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: /Quiz
        public async Task<IActionResult> Index()
        {
            try
            {
                var quizzes = await _repository.GetAllQuizzesAsync();
                return View(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading quizzes.");
                return StatusCode(500, "An error occurred while fetching quizzes.");
            }
        }

        // GET: /Quiz/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var quiz = await _repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    _logger.LogWarning("Quiz with ID {Id} not found.", id);
                    return NotFound();
                }

                return View(quiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz details for ID {Id}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // GET: /Quiz/Create
        public IActionResult Create() => View();

        // POST: /Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.AddQuizAsync(quiz);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating quiz.");
                    return StatusCode(500, "Error saving quiz.");
                }
            }
            return View(quiz);
        }

        // GET: /Quiz/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _repository.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        // POST: /Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Quiz quiz)
        {
            if (id != quiz.QuizId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateQuizAsync(quiz);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating quiz with ID {Id}", id);
                    return StatusCode(500, "Error updating quiz.");
                }
            }
            return View(quiz);
        }

        // GET: /Quiz/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _repository.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        // POST: /Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.DeleteQuizAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz with ID {Id}", id);
                return StatusCode(500, "Error deleting quiz.");
            }
        }
    }
}