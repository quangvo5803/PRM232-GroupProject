using AutoMapper;
using BusinessObject.DTOs.FeedBack;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Application;
using DataAccess.Entities.Authorize;
using DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Utilities.Exceptions;

namespace BusinessObject.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<FeedbackDto>> GetAllFeedbackAsync(int productId)
        {
            var feedbacks = (await _unitOfWork.Feedback
                .GetRangeAsync(f => f.ProductId == productId, includeProperties: "Images"))
                .ToList();

            var userIds = feedbacks.Select(f => f.UserId.ToString()).Distinct().ToList();

            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);
           
            var feedbackDtos = feedbacks.Select(f =>
            {
                var dto = _mapper.Map<FeedbackDto>(f);
                if (users.TryGetValue(f.UserId.ToString(), out var user))
                {
                    dto.FullName = user.FullName ?? "Unknown";
                    dto.Email = user.Email ?? "Unknown";
                }

                return dto;
            }).ToList();

            return feedbackDtos;
        }

        public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateRequestDto requestDto)
        {
            var rsMapper = _mapper.Map<Feedback>(requestDto);
            await _unitOfWork.Feedback.AddAsync(rsMapper);
            await _unitOfWork.SaveAsync();

            if (requestDto.Images != null && requestDto.Images.Any())
            {
                foreach (var file in requestDto.Images)
                {
                    var image = new ItemImage { FeedbackId = rsMapper.Id, ImageUrl = string.Empty };
                    await _unitOfWork.ItemImage.UploadImageAsync(file, "FoodHub/Feedback", image);
                }
            }
            return _mapper.Map<FeedbackDto>(rsMapper);
        }


        public async Task<AdminProductFeedbackDto> GetFeedbackProductAdminAsync(int productId)
        {
            var product = await _unitOfWork.Product
                .GetAsync(p => p.Id == productId, includeProperties: "ProductAvatar");

            if (product == null)
            {
                throw new CustomValidationException(new Dictionary<string, string[]>
                {
                    { "Product", new[] { "Product not found." } },
                });
            }

            var mapperProduct = _mapper.Map<AdminProductFeedbackDto>(product);

            var feedbacks = (await _unitOfWork.Feedback
                .GetRangeAsync(f => f.ProductId == productId, includeProperties: "Images")).ToList();

            var userIds = feedbacks.Select(f => f.UserId.ToString()).Distinct().ToList();

            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var feedbackDtos = feedbacks.Select(f =>
            {
                var dto = _mapper.Map<FeedbackDto>(f);
                if (users.TryGetValue(f.UserId.ToString(), out var user))
                {
                    dto.FullName = user.FullName ?? "Unknown";
                    dto.Email = user.Email ?? "Unknown";
                }

                return dto;
            }).ToList();

            mapperProduct.Feedbacks = feedbackDtos;

            return mapperProduct;
        }
    }
}
