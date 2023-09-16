using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShopping.Migrations
{
    /// <inheritdoc />
    public partial class initialSeedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Восточная кухня", "Плов — блюдо восточной кухни, основу которого составляет варёный. Отличительным свойством плова является его рассыпчатость, достигаемая соблюдением технологии приготовления риса и добавлением в плов животного или растительного жира, препятствующего слипанию крупинок.", "https://s1.eda.ru/StaticContent/Photos/120131082439/160124115932/p_O.jpg", "Плов", 7.9900000000000002, "" },
                    { 2, "Восточная кухня", "Лагман — густой азиатский суп с лапшой, мясом и овощами. Его легко превратить в первое или второе блюдо, просто изменив количество жидкости. Сегодня я поделюсь с вами рецептом супа-лагмана. Рассмотрим простой вариант с готовой лапшой!", "https://cdn.lifehacker.ru/wp-content/uploads/2022/11/303_1668514719-scaled-e1668514774627-1280x640.jpg", "Суп Лагман с говядиной", 8.9900000000000002, "" },
                    { 3, "Десерт", "Павлова — торт-безе со свежими фруктами, особенно популярный в Новой Зеландии и Австралии. Изготавливается из безе, взбитых сливок, верхний слой — из ягод или кусочков тропических фруктов. Выпекают «Павлову» в виде торта или порционно, украшая каждую порцию отдельно.", "https://klopotenko.com/wp-content/uploads/2021/03/pavlova_sitewebukr-1-1000x600.jpg", "Павлова", 8.9900000000000002, "Хит продаж" },
                    { 4, "Пицца", "Пицца для настоящих гурманов подается в ресторанах самого высокого уровня. Делается она с использованием материнских дрожжей и, зачастую, муки отличной от муки из мягких сортов пшеницы. В качестве ингредиентов для начинки используются необычные для пиццы продукты, например, бобы, лимон, инжир, дичь. Тесто получается очень легким и подается такая пицца уже нарезанной", "https://kda.ilpatio.ru/wa-data/public/shop/products/38/15/31538/images/15375/15375.900x616.jpg", "Пицца-гурме", 10.99, "" },
                    { 5, "Восточная кухня", "Казан-кебаб — это несложное блюдо, которое состоит из двух основных ингредиентов, но получается вкусным и сытным. Особый анисово-терпкий аромат казан-кебабу обеспечивает лаконичный набор пряностей — кориандр и зира.", "https://www.vsegdavkusno.ru/assets/images/recipes/2443/okazalos-prosto-poluchaetsja-volshebno-kazan-kebab-na-kostre-ili-na-plite-low.jpg", "Казан-кебаб", 12.99, "Самые популярные" },
                    { 6, "Грузинская кухня", "Хинкали - это национальное грузинское блюдо. Ни в коем случае их не стоит сравнивать с пельменями, это кардинально другое блюдо, одновременно состоящее из первого и второго блюда. Потрясающе вкусная вещь!", "https://s1.eda.ru/StaticContent/Photos/120214124236/151226153157/p_O.jpg", "Хинкали", 13.99, "Специальное предложение от шеф-повара" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
