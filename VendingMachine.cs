using System;
using System.Collections.Generic;
using System.Linq;

public class VendingMachine
{
    private List<Product> products = new List<Product>();
    private int[] coins = { 1, 2, 5, 10 };
    private decimal insertedMoney = 0;  
    private decimal totalSales = 0;

    public VendingMachine()
    {
        products.Add(new Product(1, "Вода 0.5л", 40.0m, 10));
        products.Add(new Product(2, "Чипсы", 75.0m, 5));
        products.Add(new Product(3, "Шоколад", 90.0m, 5));
        products.Add(new Product(4, "Кофе", 60.0m, 8));
    }

    public void Run()
    {
        while (true)
        {
            string[] mainMenu = {
                "\n--- Вендинговый автомат ---",
                "1. Показать товары",
                "2. Вставить монету",
                "3. Выбрать товар",
                "4. Отменить и вернуть деньги",
                "5. Админ меню",
                "0. Выход"
            };

            foreach (var line in mainMenu)
                Console.WriteLine(line);

            Console.Write("Ваш выбор: ");
            string? choice = Console.ReadLine();

            if (choice == "1") ShowProducts();
            else if (choice == "2") InsertCoin();
            else if (choice == "3") BuyProduct();
            else if (choice == "4") Cancel();
            else if (choice == "5") AdminMenu();
            else if (choice == "0") break;
            else Console.WriteLine("Неверный ввод.");
        }
    }

    private void ShowProducts()
    {
        Console.WriteLine("\nТовары:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} — {p.Price:F2} руб. (в наличии: {p.Quantity})");
        }
        Console.WriteLine($"Внесено: {insertedMoney:F2} руб.");
    }

    private void InsertCoin()
    {
        Console.WriteLine("\nДоступные монеты: 1, 2, 5, 10 руб.");
        Console.Write("Введите номинал монеты: ");
        string? input = Console.ReadLine();
        if (input != null && int.TryParse(input, out int coin) && Array.Exists(coins, c => c == coin))
        {
            insertedMoney += coin;
            Console.WriteLine($"Вы внесли {coin} руб. Всего внесено: {insertedMoney:F2} руб.");
        }
        else
        {
            Console.WriteLine("Такой монеты нет.");
        }
    }

    private void BuyProduct()
    {
        Console.Write("Введите id товара: ");
        string? input = Console.ReadLine();
        if (input == null || !int.TryParse(input, out int id)) return;

        var product = products.Find(p => p.Id == id);
        if (product == null)
        {
            Console.WriteLine("Нет такого товара.");
            return;
        }
        if (product.Quantity <= 0)
        {
            Console.WriteLine("Товар закончился.");
            return;
        }
        if (insertedMoney < product.Price)
        {
            Console.WriteLine($"Недостаточно денег. Цена: {product.Price:F2} руб.");
            return;
        }

        product.Quantity--;
        insertedMoney -= product.Price;
        totalSales += product.Price;

        Console.WriteLine($"Вы купили: {product.Name}");
        if (insertedMoney > 0)
        {
            GiveChange();
        }
    }

    private void GiveChange()
    {
        decimal change = insertedMoney;
        int[] coinsDesc = coins.OrderByDescending(c => c).ToArray();
        Console.Write("Сдача: ");
        foreach (int c in coinsDesc)
        {
            int count = (int)(change / c);
            if (count > 0)
            {
                Console.Write($"{count}x{c} ");
                change -= count * c;
            }
        }
        Console.WriteLine();
        insertedMoney = 0;
    }

    private void Cancel()
    {
        if (insertedMoney > 0)
        {
            GiveChange();
        }
        else
        {
            Console.WriteLine("Вы ничего не внесли.");
        }
    }

    private void AdminMenu()
    {
        Console.Write("Введите пароль администратора: ");
        string? pass = Console.ReadLine();
        if (pass != "admin")
        {
            Console.WriteLine("Неверный пароль.");
            return;
        }

        string[] adminMenu = {
            "\n--- Админ меню ---",
            "1. Пополнить товар",
            "2. Добавить новый товар",
            "3. Посмотреть выручку",
            "0. Выход"
        };

        while (true)
        {
            foreach (var line in adminMenu)
                Console.WriteLine(line);

            Console.Write("Ваш выбор: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                ShowProducts();
                Console.Write("Введите id товара: ");
                string? input = Console.ReadLine();
                if (input != null && int.TryParse(input, out int id))
                {
                    var product = products.Find(p => p.Id == id);
                    if (product != null)
                    {
                        Console.Write("Сколько добавить: ");
                        string? countInput = Console.ReadLine();
                        if (countInput != null && int.TryParse(countInput, out int count))
                        {
                            product.Quantity += count;
                            Console.WriteLine("Товар пополнен.");
                        }
                    }
                }
            }
            else if (choice == "2")
            {
                int newId = products.Any() ? products.Max(p => p.Id) + 1 : 1;

                Console.Write("Название: ");
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) name = "Без имени";

                Console.Write("Цена: ");
                string? priceInput = Console.ReadLine();
                if (priceInput == null || !decimal.TryParse(priceInput, out decimal price)) price = 0;

                Console.Write("Количество: ");
                string? qtyInput = Console.ReadLine();
                if (qtyInput == null || !int.TryParse(qtyInput, out int qty)) qty = 0;

                products.Add(new Product(newId, name, price, qty));
                Console.WriteLine("Товар добавлен.");
            }
            else if (choice == "3")
            {
                Console.WriteLine($"Выручка: {totalSales:F2} руб.");
            }
            else if (choice == "0") break;
        }
    }
}
