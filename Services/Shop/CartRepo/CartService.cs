using Data.Entities.Enums;
using Data.Entities.Shop;
using Data.Entities.User;
using Microsoft.AspNetCore.Components;
using Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shop.CategoryRepo
{
    public class CartService : ICartService
    {
        private readonly IRepository<ShoppingCart> _repository;
        public CartService(IRepository<ShoppingCart> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ShoppingCart> GetMany(Expression<Func<ShoppingCart, bool>> expression, List<string> references)
        {
            return _repository.GetAll(expression, references);
        }

        public async Task<ShoppingCart> GetOne(Expression<Func<ShoppingCart, bool>> expression, List<string> references)
        {
            return await _repository.Get(expression, references);
        }

        public bool Insert(ShoppingCart model)
        {
            try
            {
                if (model is not null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    _repository.Insert(model);
                }
                _repository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public void Update(ShoppingCart model)
        {
            if (model is not null)
            {
                _repository.Update(model);
            }
            _repository.SaveChanges();
        }
        public async Task Delete(int id)
        {
            ShoppingCart model = await GetOne(s => s.ID == id, null);

            _repository.Delete(model);
            _repository.SaveChanges();
        }

        public ShoppingCart AddToShopCart(AppUser user, Product product, ShoppingCart shoppingCart)
        {
            try
            {
                //if available cart exists and not completed yet
                if (shoppingCart != null && (shoppingCart.StatusOfCompletion == nameof(ShoppingCartStatus.PendingForPreview)))
                {
                    //if same product added exists already in database
                    var existProduct = shoppingCart.ShoppingCartItems.FirstOrDefault(a => a.ProductID == product.ID);
                    if (existProduct == null)
                    {
                        shoppingCart.ShoppingCartItems.Add(new ShoppingCartItem
                        {
                            Product = product,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow,
                            Quantity = 1,
                            ProductID = product.ID,
                        });
                    }
                    else
                    {
                        existProduct.ModifiedDate = DateTime.UtcNow;
                        existProduct.Quantity += 1;
                    }

                    return shoppingCart;
                }
                else
                {
                    var newShoppingCart = new ShoppingCart()
                    {
                        AppUser = user,
                        AppUserId = user.Id,
                        StatusOfCompletion = ShoppingCartStatus.PendingForPreview.ToString(),
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        ShoppingCartItems = new List<ShoppingCartItem>(),
                    };
                    newShoppingCart.ShoppingCartItems.Add(new ShoppingCartItem
                    {
                        Product = product,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        Quantity = 1,
                        ProductID = product.ID,
                    });
                    return newShoppingCart;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
          
        }
    }
}
