using AutoMapper;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.BLL.DTO;
using FoodDelivery.BLL.Interfaces;

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

            return _mapper.Map<List<DishDTO>>(dishes);
        }

        public async Task<List<DishDTO>> GetByCategoryAsync(DishCategory category)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            var filtered = dishes.Where(d => d.Category == category);

            return _mapper.Map<List<DishDTO>>(filtered);
        }

        public async Task<List<DishDTO>> GetByDayAsync(DayOfWeek day)
        {
            var dishes = await _uow.Dishes.GetAllAsync();

            var filtered = dishes.Where(d => d.DayOfWeek == day);

            return _mapper.Map<List<DishDTO>>(filtered);
        }
    }
}