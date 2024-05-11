using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab78.Pages
{

    public class Game
    {
        public Game()
        {
            // конструктор без параметров
        }

        public Game(int id, int genre_id, string game_name)
        {
            this.id = id;
            this.genre_id = genre_id;
            this.game_name = game_name;
        }

        public int id { get; set; }
        public int genre_id { get; set; }
        public string game_name { get; set; }
    }

    public class IndexModel : PageModel
    {

        MyDbContext _dbContext;

        public IndexModel(MyDbContext dbContext)
        {
            Console.WriteLine("init");
            _dbContext = dbContext;
            games = _dbContext.game.ToList();
        }

        public List<Game> games { get; set; }

        [BindProperty]
        public GameFormModel gameForm { get; set; }

        public bool ShowForm { get; set; }
        public bool ShowCreatingForm { get; set; }

        public void OnGet()
        {

        }

        public async void OnPostEditGame(int id)
        {
            var game = _dbContext.game.Where((it) => it.id == id).First();


            gameForm = new GameFormModel
            {
                id = game.id,
                gameName = game.game_name,
                genreId = game.genre_id
            };


            ShowForm = true;
            ShowCreatingForm = false;

        }
        public async Task<IActionResult> OnPostDeleteGame(int id)
        {
            var game = _dbContext.game.Where((it) => it.id == id).First();
            var gamePublishers = _dbContext.game_publisher.Where((it) => it.game_id == id).ToList();
            var gamePlatforms = _dbContext.game_platform.Where((it) => gamePublishers.Select((it) => it.id).Contains(it.game_publisher_id)).ToList();

            Console.WriteLine(gamePublishers.Count());

            _dbContext.game_platform.RemoveRange(gamePlatforms);
            _dbContext.game_publisher.RemoveRange(gamePublishers);
            _dbContext.SaveChanges();

            _dbContext.game.Remove(game);
            _dbContext.SaveChanges();

            return RedirectToPage("/Index");
        }

        public async void OnPostEditSaveGame(int id)
        {


            var game = _dbContext.game.Where((it) => it.id == id).First();
            if (_dbContext.genre.Where((it) => it.id == gameForm.genreId).Count() == 0)
            {
                return;
            }

            game.game_name = gameForm.gameName;
            game.genre_id = gameForm.genreId;

            Console.WriteLine(game.id);

            _dbContext.game.Update(game);
            _dbContext.SaveChanges();

            ShowForm = false;

        }

        public async void OnPostEditCancelGame()
        {
            ShowForm = false;
        }

        public async void OnPostAddGame()
        {
            ShowCreatingForm = true;
        }

        public async Task<IActionResult> OnPostCreateSaveGame(int id)
        {
            _dbContext.game.Add(
                new Game(
                    id: Guid.NewGuid().GetHashCode(),
                    game_name: gameForm.gameName,
                    genre_id: gameForm.genreId
                )
            );
            _dbContext.SaveChanges();

            ShowCreatingForm = false;

            return RedirectToPage("/Index");
        }

        public async void OnPostCreateCancelGame()
        {
            ShowCreatingForm = false;
        }

        public class GameFormModel
        {
            public int id { get; set; }

            public string gameName { get; set; }

            public int genreId { get; set; }

        }

    }
}
