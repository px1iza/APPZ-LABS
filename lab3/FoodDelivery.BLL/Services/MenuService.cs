using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // 📌 меню на день
        public async Task<List<DishDTO>> GetMenuByDayAsync(DayOfWeek day)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            var result = dishes.Where(d => d.DayOfWeek == day);

            return _mapper.Map<List<DishDTO>>(result);
        }

        // 🍱 комплексний обід (по 1 страві з категорії)
        public async Task<List<DishDTO>> GetComplexLunchAsync(DayOfWeek day)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            var result = dishes
                .Where(d => d.DayOfWeek == day)
                .GroupBy(d => d.Category)
                .Select(g => g.First());

            return _mapper.Map<List<DishDTO>>(result);
        }

        // 🔎 пошук по категорії
        public async Task<List<DishDTO>> GetByCategoryAsync(DishCategoryDTO category)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            var result = dishes.Where(d => (DishCategoryDTO)d.Category == category);

            return _mapper.Map<List<DishDTO>>(result);
        }
    }
}