using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace ProjectService.API.Infrastructure.Persistence;

interface IProjectTemplateRepository
{
    Task<Result<List<ProjectCreationWizardTemplate>>> GetProjectCreationWizardTemplatesByCategoryName(string categoryName);
    Task<Result<List<ProjectCreationWizardTemplateCategory>>> GetProjectCreationWizardTemplatesCategories();

}
class ProjectTemplateRepository : IProjectTemplateRepository
{
    private readonly ApplicationDbContext _context;
    ProjectTemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<List<ProjectCreationWizardTemplate>>> GetProjectCreationWizardTemplatesByCategoryName(string categoryName)
    {
        var query = _context.Categories.Include(c => c.ProjectTemplates);
        var category = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(query, c => c.Name == categoryName);
        if(category == null) return Result<List<ProjectCreationWizardTemplate>>.Failure(
                Error.NotFound(ErrorCode.NotFound, "Project Templates not found", "Project Templates not found"));
        

        return Result<List<ProjectCreationWizardTemplate>>.Success(category.ProjectTemplates.ToList());
    }

    public async Task<Result<List<ProjectCreationWizardTemplateCategory>>> GetProjectCreationWizardTemplatesCategories()
    {
        var categories = await EntityFrameworkQueryableExtensions.ToListAsync(_context.Categories);
        if(!categories.Any()) return Result<List<ProjectCreationWizardTemplateCategory>>.Failure(Error.NotFound(ErrorCode.NotFound, "Categories not found", "Categories not found"));

        return Result<List<ProjectCreationWizardTemplateCategory>>.Success(categories.ToList());
    }
}