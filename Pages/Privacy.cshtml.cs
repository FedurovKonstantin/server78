using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab78.Pages
{


    public class PrivacyModel : PageModel
    {

        MyDbContext _dbContext;

        public PrivacyModel(MyDbContext dbContext)
        {
            Console.WriteLine("init");
            _dbContext = dbContext;
            publishers = _dbContext.publisher.ToList();
        }

        public List<Publisher> publishers { get; set; }

        [BindProperty]
        public PublisherFormModel publisherForm { get; set; }

        public bool ShowForm { get; set; }
        public bool ShowCreatingForm { get; set; }

        public void OnGet()
        {

        }

        public async void OnPostEditPublisher(int id)
        {
            var publisher = _dbContext.publisher.Where((it) => it.id == id).First();


            publisherForm = new PublisherFormModel
            {
                id = publisher.id,
                name = publisher.publisher_name
            };

            ShowForm = true;
            ShowCreatingForm = false;

        }
        public async Task<IActionResult> OnPostDeletePublisher(int id)
        {
            var publisher = _dbContext.publisher.Where((it) => it.id == id).First();
            var gamePublishers = _dbContext.game_publisher.Where((it) => it.publisher_id == id).ToList();
            var gamePlatforms = _dbContext.game_platform.Where((it) => gamePublishers.Select((it) => it.id).Contains(it.game_publisher_id)).ToList();

            _dbContext.game_platform.RemoveRange(gamePlatforms);
            _dbContext.game_publisher.RemoveRange(gamePublishers);
            _dbContext.SaveChanges();

            _dbContext.publisher.Remove(publisher);
            _dbContext.SaveChanges();

            return RedirectToPage("/Privacy");
        }

        public async void OnPostEditSavePublisher(int id)
        {


            var publisher = _dbContext.publisher.Where((it) => it.id == id).First();
            if (_dbContext.publisher.Where((it) => it.id == publisher.id).Count() == 0)
            {
                return;
            }

            publisher.publisher_name = publisherForm.name;

            Console.WriteLine(publisher.publisher_name);

            _dbContext.publisher.Update(publisher);
            _dbContext.SaveChanges();

            ShowForm = false;
        }

        public async void OnPostEditCancelPublisher()
        {
            ShowForm = false;
        }

        public async void OnPostAddPublisher()
        {
            ShowCreatingForm = true;
        }

        public async Task<IActionResult> OnPostCreateSavePublisher(int id)
        {
            _dbContext.publisher.Add(
                new Publisher(
                    id: Guid.NewGuid().GetHashCode(),
                    publisher_name: publisherForm.name
                )
            );
            _dbContext.SaveChanges();

            ShowCreatingForm = false;

            return RedirectToPage("/Privacy");
        }

        public async void OnPostCreateCancelPublisher()
        {
            ShowCreatingForm = false;
        }

        public class PublisherFormModel
        {
            public int id { get; set; }

            public string name { get; set; }
        }

    }
}
