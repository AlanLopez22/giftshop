using GiftShop.Entities;
using GiftShop.Models;

namespace GiftShop
{
    public static class EntitiesExtensions
    {
        public static void Update(this Product entity, ProductModel viewModel)
        {
            entity.ID = viewModel.ID;
            entity.Description = viewModel.Description;
            entity.CategoryID = viewModel.CategoryID;
            entity.Name = viewModel.Name;
            entity.Price = viewModel.Price;
            entity.IsActive = viewModel.IsActive;
        }
    }
}