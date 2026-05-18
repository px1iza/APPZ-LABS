using AutoMapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Enum;

namespace FoodDelivery.BLL.Services
{
    public class DishService : IDishService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DishService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<DishDTO>> GetAllAsync()
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            if (dishes == null || !dishes.Any())
                throw new Exception("Страви не знайдено в системі");

            return _mapper.Map<List<DishDTO>>(dishes);
        }

        public async Task<List<DishDTO>> GetByCategoryAsync(DishCategoryEnum category)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            if (dishes == null || !dishes.Any())
                throw new Exception("Страви не знайдено в системі");

            var filtered = dishes
                .Where(d => d.Category == (DAL.Entities.DishCategory)(int)category)
                .ToList();

            if (!filtered.Any())
                throw new Exception($"Страви категорії {category} не знайдено");

            return _mapper.Map<List<DishDTO>>(filtered);
        }

        public async Task<List<DishDTO>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Ключове слово не може бути порожнім");

            var dishes = await _uow.Dishes.GetAllAsync();

            if (dishes == null || !dishes.Any())
                throw new Exception("Страви не знайдено в системі");

            var filtered = dishes
                .Where(d => d.Title.ToLower().Contains(keyword.ToLower()))
                .ToList();

            if (!filtered.Any())
                throw new Exception($"Страви за запитом '{keyword}' не знайдено");

            return _mapper.Map<List<DishDTO>>(filtered);
        }
    }
}