using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Soccer.Hubs;
using Soccer.Models;
using System.Text.RegularExpressions;

namespace Soccer.Controllers
{
    public class PlayersController : Controller
    {
        private readonly SoccerContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IHubContext<PlayersHub> _hubContext; // !!!

        public PlayersController(SoccerContext context, ISieveProcessor sieveProcessor, IHubContext<PlayersHub> hubContext) // !!!
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _hubContext = hubContext; // !!!
        }

        // GET: Players
        // !!! метод Index залишається без змін, бо він вже робить свою роботу: повертає свіжі дані з БД
        // SignalR працює окремо: сервер штовхає "PlayersUpdated", клієнт(JS в Index.cshtml) ловить і робить location.reload(), а потім браузер запитує Index заново та отримує актуальні дані
        // коротше, вся AJAX-логіка оновлення списку гравців винесена в JS-код на стороні клієнта
        public async Task<IActionResult> Index([FromQuery] SieveModel sieveModel)
        {
            // отримуємо поточні фільтри та сортування з рядка запиту
            var currentFilters = Request.Query["filters"].ToString();
            var currentSorts = Request.Query["sorts"].ToString();

            var query = _context.Players
                .Include(p => p.Team)
                .AsNoTracking(); // AsNoTracking - для покращення продуктивності при читанні, конкретно робить запити "тільки для читання"

            // впроваджуємо Sieve
            var processedQuery = _sieveProcessor.Apply(sieveModel, query);

            var countQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false); // pplyPagination: false щоб отримати загальну кількість записів без урахування пагінації
            var totalCount = await countQuery.CountAsync();

            var players = await processedQuery.ToListAsync();

            // на вью треба передати список команд для фільтрації
            ViewBag.Teams = await _context.Teams.OrderBy(t => t.Name).ToListAsync();

            // витягаємо поточний TeamId з фільтрів для підсвічування вибраної команди у фільтрі
            var currentTeamId = 0;
            if (!string.IsNullOrEmpty(currentFilters))
            {
                var match = Regex.Match(currentFilters, @"TeamId==(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out var id))
                {
                    currentTeamId = id;
                }
            }
            ViewBag.CurrentTeamId = currentTeamId;

            var viewModel = new IndexViewModel
            {
                Players = players,
                TotalCount = totalCount,
                Page = sieveModel.Page ?? 1,
                PageSize = sieveModel.PageSize ?? 5
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Position,TeamId")] Player player)
        {
            try
            {
                _context.Add(player);
                await _context.SaveChangesAsync();

                // !!! повідомляємо всіх клієнтів про оновлення
                await _hubContext.Clients.All.SendAsync("PlayersUpdated");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Position,TeamId")] Player player)
        {
            if (id != player.Id) return NotFound();

            try
            {
                _context.Update(player);
                await _context.SaveChangesAsync();

                // !!! повідомляємо всіх клієнтів про оновлення
                await _hubContext.Clients.All.SendAsync("PlayersUpdated");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayersExists(player.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();

                // !!! повідомляємо всіх клієнтів про видалення
                await _hubContext.Clients.All.SendAsync("PlayersUpdated");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PlayersExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}