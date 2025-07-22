using BusinessObject.Services.Interfaces;

namespace BusinessObject.FacadeService
{
    public interface IFacadeService 
    {
        IShoppingCartService ShoppingCartService { get; }
    }
}
