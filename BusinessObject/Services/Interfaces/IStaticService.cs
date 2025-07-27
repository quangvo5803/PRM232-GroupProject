using BusinessObject.DTOs.Admin;

namespace BusinessObject.Services.Interfaces
{
    public interface IStaticService
    {
        Task <StaticDto> GetStatisticAsync();
    }
}
