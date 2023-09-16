using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Models;

namespace OnlineShopping.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext (DbContextOptions options)
          : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Плов",
                    Description = "Плов — блюдо восточной кухни, основу которого составляет варёный. Отличительным свойством плова является его рассыпчатость, достигаемая соблюдением технологии приготовления риса и добавлением в плов животного или растительного жира, препятствующего слипанию крупинок.",
                    Image = "https://s1.eda.ru/StaticContent/Photos/120131082439/160124115932/p_O.jpg",
                    Price = 7.99,
                    Category = "Восточная кухня",
                    SpecialTag = ""
                }, new Product
                {
                    Id = 2,
                    Name = "Суп Лагман с говядиной",
                    Description = "Лагман — густой азиатский суп с лапшой, мясом и овощами. Его легко превратить в первое или второе блюдо, просто изменив количество жидкости. Сегодня я поделюсь с вами рецептом супа-лагмана. Рассмотрим простой вариант с готовой лапшой!",
                    Image = "https://cdn.lifehacker.ru/wp-content/uploads/2022/11/303_1668514719-scaled-e1668514774627-1280x640.jpg",
                    Price = 8.99,
                    Category = "Восточная кухня",
                    SpecialTag = ""
                }, new Product
                {
                    Id = 3,
                    Name = "Павлова",
                    Description = "Павлова — торт-безе со свежими фруктами, особенно популярный в Новой Зеландии и Австралии. Изготавливается из безе, взбитых сливок, верхний слой — из ягод или кусочков тропических фруктов. Выпекают «Павлову» в виде торта или порционно, украшая каждую порцию отдельно.",
                    Image = "https://klopotenko.com/wp-content/uploads/2021/03/pavlova_sitewebukr-1-1000x600.jpg",
                    Price = 8.99,
                    Category = "Десерт",
                    SpecialTag = "Хит продаж"
                }, new Product
                {
                    Id = 4,
                    Name = "Пицца-гурме",
                    Description = "Пицца для настоящих гурманов подается в ресторанах самого высокого уровня. Делается она с использованием материнских дрожжей и, зачастую, муки отличной от муки из мягких сортов пшеницы. В качестве ингредиентов для начинки используются необычные для пиццы продукты, например, бобы, лимон, инжир, дичь. Тесто получается очень легким и подается такая пицца уже нарезанной",
                    Image = "https://kda.ilpatio.ru/wa-data/public/shop/products/38/15/31538/images/15375/15375.900x616.jpg",
                    Price = 10.99,
                    Category = "Пицца",
                    SpecialTag = ""
                }, new Product
                {
                    Id = 5,
                    Name = "Казан-кебаб",
                    Description = "Казан-кебаб — это несложное блюдо, которое состоит из двух основных ингредиентов, но получается вкусным и сытным. Особый анисово-терпкий аромат казан-кебабу обеспечивает лаконичный набор пряностей — кориандр и зира.",
                    Image = "https://www.vsegdavkusno.ru/assets/images/recipes/2443/okazalos-prosto-poluchaetsja-volshebno-kazan-kebab-na-kostre-ili-na-plite-low.jpg",
                    Price = 12.99,
                    Category = "Восточная кухня",
                    SpecialTag = "Самые популярные"
                }, new Product
                {
                    Id = 6,
                    Name = "Хинкали",
                    Description = "Хинкали - это национальное грузинское блюдо. Ни в коем случае их не стоит сравнивать с пельменями, это кардинально другое блюдо, одновременно состоящее из первого и второго блюда. Потрясающе вкусная вещь!",
                    Image = "https://s1.eda.ru/StaticContent/Photos/120214124236/151226153157/p_O.jpg",
                    Price = 13.99,
                    Category = "Грузинская кухня",
                    SpecialTag = "Специальное предложение от шеф-повара"
                }
              );
        }
    }
}
