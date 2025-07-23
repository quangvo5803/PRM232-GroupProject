using AutoMapper;
using BusinessObject.Services;
using BusinessObject.Services.Interfaces;
<<<<<<< HEAD
using DataAccess.Repositories.Interfaces;
=======
>>>>>>> develop
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

<<<<<<< HEAD
        public IShoppingCartService ShoppingCartService { get; }

=======
        public ICategoryService Category { get; private set; }
        public IProductService Product { get; private set; }
>>>>>>> develop

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
<<<<<<< HEAD
            ShoppingCartService = new ShoppingCartService(_unitOfWork, _mapper);
=======
            Category = new CategoryService(_unitOfWork, _mapper);
            Product = new ProductService(_unitOfWork, _mapper);
>>>>>>> develop
        }
    }
}
