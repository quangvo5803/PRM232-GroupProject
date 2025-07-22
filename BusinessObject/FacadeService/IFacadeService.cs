using BusinessObject.Services.Interfaces;
using DataAccess.Repositories.Interfaces;

namespace BusinessObject.FacadeService
{
    public interface IFacadeService
    {
        ICategoryService Category { get; }
        IProductService Product { get; }
    }
}
