using AutoMapper;
using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var cacheKey = "All_Categories";

          
            var cachedCategories = await _cacheService.GetCachedDataAsync<IEnumerable<CategoryDto>>(cacheKey);
            if (cachedCategories != null)
                return cachedCategories;

          
            var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
            var response = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            await _cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromDays(1));

            return response;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var cacheKey = $"Category_{id}";

            var cachedCategory = await _cacheService.GetCachedDataAsync<CategoryDto>(cacheKey);
            if (cachedCategory != null)
                return cachedCategory;

            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with ID {id} was not found.");

            var response = _mapper.Map<CategoryDto>(category);

            await _cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromDays(1));

            return response;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await _unitOfWork.Repository<Category, int>().AddAsync(category);
            await _unitOfWork.CompleteAsync();

            await _cacheService.RemoveCachedDataAsync("All_Categories");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with ID {id} was not found.");

            _mapper.Map(updateCategoryDto, category);
            _unitOfWork.Repository<Category, int>().Update(category);
            await _unitOfWork.CompleteAsync();

           
            await _cacheService.RemoveCachedDataAsync("All_Categories");
            await _cacheService.RemoveCachedDataAsync($"Category_{id}");

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with ID {id} was not found.");

            _unitOfWork.Repository<Category, int>().Delete(category);
            await _unitOfWork.CompleteAsync();

           
            await _cacheService.RemoveCachedDataAsync("All_Categories");
            await _cacheService.RemoveCachedDataAsync($"Category_{id}");

            return true;
        }
    }
}