using AutoMapper;
using BusinessObject.Services;
using BusinessObject.Services.Interfaces;
using DataAccess.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Utilities.Email.Interface;

namespace BusinessObject.FacadeService
{
    public class FacadeService : IFacadeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailQueue _emailQueue;
        private readonly IMapper _mapper;

        public ICategoryService Category { get; private set; }
        public IProductService Product { get; private set; }

        public FacadeService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IEmailQueue emailQueue,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailQueue = emailQueue;
            _mapper = mapper;
            Category = new CategoryService(_unitOfWork, _mapper);
            Product = new ProductService(_unitOfWork, _mapper);
        }
    }
}
