using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareWebApi.Services.MasterData;

public interface IDietaryTagService
{
    Task<IDietaryTag> GetAsync(long id);
    Task<IEnumerable<IDietaryTag>> GetListAsync(CancellationToken cancellationToken);
    Task<IDietaryTag> SaveAsync(IDietaryTag save);
}