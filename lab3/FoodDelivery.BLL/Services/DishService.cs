using AutoMapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Entities;

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
        // 🍽 Отримати всі страви
        public async Task<List<DishDTO>> GetAllAsync()
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            return _mapper.Map<List<DishDTO>>(dishes);
        }

        // 🔍 Отримати по ID

        public async Task<DishDTO?> GetByIdAsync(int id)
        {
            var dish = await _uow.Dishes.GetByIdAsync(id);
            if (dish == null)

                return null;

            return _mapper.Map<DishDTO>(dish);
        }

        // 🧾 Фільтр по категорії

        public async Task<List<DishDTO>> GetByCategoryAsync(DishCategory category)
        {
            var dishes = await _uow.Dishes.GetAllAsync();
            var filtered = dishes
                .Where(d => d.Category == category)
                .ToList();
            return _mapper.Map<List<DishDTO>>(filtered);

        }

        public async Task<List<DishDTO>> SearchAsync(string keyword)
        {
            var dishes = await _uow.Dishes.GetAllAsync();
            var filtered = dishes
                .Where(d => d.Title.ToLower().Contains(keyword.ToLower()))
                .ToList();
            return _mapper.Map<List<DishDTO>>(filtered);
        }
    }
}