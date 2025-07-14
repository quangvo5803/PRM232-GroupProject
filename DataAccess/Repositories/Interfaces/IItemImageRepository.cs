using DataAccess.Entities.Application;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Repositories.Interfaces
{
    public interface IItemImageRepository : IRepository<ItemImage>
    {
        Task UploadImageAsync(IFormFile file, string folder, ItemImage Image);
        Task<bool> DeleteImageAsync(string publicId);
    }
}
