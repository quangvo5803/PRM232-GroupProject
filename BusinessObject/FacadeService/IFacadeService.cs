using BusinessObject.Services.Interfaces;
using DataAccess.Repositories.Interfaces;

namespace BusinessObject.FacadeService
{
    public interface IFacadeService
    {
        ICategoryService Category { get; }
        IOrderService Order { get; }
        IProductService Product { get; }
        IShoppingCartService ShoppingCart{ get; }
    }
}
