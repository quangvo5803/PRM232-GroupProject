using AutoMapper;
using BusinessObject.Services;
using BusinessObject.Services.Interfaces;

using DataAccess.Repositories.Interfaces;

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
        private readonly IVnPayService _vpnPayService;


        public IShoppingCartService ShoppingCart{ get; private set; }

        public ICategoryService Category { get; private set; }
        public IOrderService Order { get; private set; }
        public IProductService Product { get; private set; }

        public FacadeService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IEmailQueue emailQueue,
            IMapper mapper,
            IVnPayService vnPayService
        )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailQueue = emailQueue;
            _mapper = mapper;
            _vpnPayService = vnPayService;
            Category = new CategoryService(_unitOfWork, _mapper);
            Order = new OrderService(_unitOfWork, _mapper, _vpnPayService);
            ShoppingCart = new ShoppingCartService(_unitOfWork, _mapper);
            Category = new CategoryService(_unitOfWork, _mapper);
            Product = new ProductService(_unitOfWork, _mapper);
        }
    }
}
