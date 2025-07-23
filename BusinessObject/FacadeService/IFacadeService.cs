using BusinessObject.Services.Interfaces;
<<<<<<< HEAD

namespace BusinessObject.FacadeService
{
    public interface IFacadeService 
    {
        IShoppingCartService ShoppingCartService { get; }
=======
using DataAccess.Repositories.Interfaces;

namespace BusinessObject.FacadeService
{
    public interface IFacadeService
    {
        ICategoryService Category { get; }
        IProductService Product { get; }
>>>>>>> develop
    }
}
