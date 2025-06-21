# MazlaySuperCar

MazlaySuperCar — интернет-магазин автозапчастей на ASP.NET Core MVC и PostgreSQL. Есть поддержка ролей, админ-функций, SignalR для мгновенного получения заказов.

## Запуск проекта

1. Клонировать репозиторий и установить зависимости:
    ```
    dotnet restore
    ```

2. Применить миграции и создать базу данных:
    ```
    dotnet ef migrations add InitPg --project Persistence --startup-project MazlaySuperCar
    dotnet ef database update --project Persistence --startup-project MazlaySuperCar
    ```

3. Запустить проект:
    ```
    dotnet run --project MazlaySuperCar
    ```
    По умолчанию сайт будет на http://localhost:5000

## Администратор

- Email: admin@gmail.com
- Пароль: adminADMIN123!

Вход в админские разделы:
- Список пользователей: `/Admin/Users`
- Список заказов: `/Admin/Orders`

(Доступ к этим страницам есть только у пользователей с ролью Admin.)

## Как пользоваться

1. Выбери нужные запчасти и добавь их в корзину или wishlist (избранное).
2. В корзине можно менять количество товаров и оформлять заказ.
3. После оформления заказа он автоматически появляется у администратора на странице `/Admin/Orders` (с помощью SignalR, без перезагрузки).
4. Администратор может просматривать и управлять заказами, а также удалять пользователей на странице `/Admin/Users`.

## Технологии

- ASP.NET Core MVC 9
- Entity Framework Core + PostgreSQL
- ASP.NET Identity (авторизация, роли)
- SignalR (уведомления о новых заказах)
- Bootstrap 5

## Быстрый старт

```sh
dotnet restore
dotnet ef migrations add InitPg --project Persistence --startup-project MazlaySuperCar
dotnet ef database update --project Persistence --startup-project MazlaySuperCar
dotnet run --project MazlaySuperCar
