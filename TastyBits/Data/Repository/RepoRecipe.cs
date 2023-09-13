using System.Linq.Expressions;
using TastyBits.Data.Repository.IRepository;
using TastyBits.Model;

namespace TastyBits.Data.Repository
{
    public class RepoRecipe:RepoBase<Recipe>,IRecipeRepo
    {
        private readonly AppDbContext _db;

        public RepoRecipe(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
