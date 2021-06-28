using Git.Data;
using Git.Data.Models;
using Git.Models.Commits;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Git.Controllers
{
    using static DataConstants;
    public class CommitsController : Controller
    {
        private readonly GitDbContext data;

        public CommitsController(GitDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = data.Repositories
                .Where(r => r.Id == id)
                .Select(r => new CommitToRepositoryViewModel
            {
                Id = r.Id,
                Name = r.Name
            })
                .FirstOrDefault();

            if (repository == null)
            {
                return BadRequest();
            }

            return this.View(repository);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateCommitToRepository model)
        {
            if (!data.Repositories.Any(r=>r.Id == model.Id))
            {
                return BadRequest();
            }

            if (model.Description.Length < MinDescriptionLength)
            {
                return Error($"Description should be more than {MinDescriptionLength} symbols."); 
            }

            var commit = new Commit
            {
                Id = model.Id,
                Description = model.Description,
                CreatorId = this.User.Id,
                
            };

            this.data.Commits.Add(commit);

            this.data.SaveChanges();

            return this.Redirect("/Repositories/All");
        }
    }
}
