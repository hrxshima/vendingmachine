using System;
using System.Collections.Generic;

public class VendingMachine
{
    private List<Product> products = new List<Product>();
    private int[] coins = { 1, 2, 5, 10 };
    private decimal insertedMoney = 0;
    private decimal totalSales = 0;

    public VendingMachine()
    {
        products.Add(new Product(1, "Вода 0.5л", 40m, 10));
        products.Add(new Product(2, "Чипсы", 75m, 5));
        products.Add(new Product(3, "Шоколад", 90m, 5));
        products.Add(new Product(4, "Кофе", 60m, 8));
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nМеню");
            Console.WriteLine("1. Показать товары");
            Console.WriteLine("2. Вставить монету");
            Console.WriteLine("3. Купить товар");
            Console.WriteLine("4. Отмена и сдача");
            Console.WriteLine("5. Админ меню");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            if (choice == "1") ShowProducts();
            else if (choice == "2") InsertCoin();
            else if (choice == "3") BuyProduct();
            else if (choice == "4") Cancel();
            else if (choice == "5") AdminMenu();
            else if (choice == "0") return;
            else Console.WriteLine("Неверный ввод.");
        }
    }

    private void ShowProducts()
    {
        Console.WriteLine("\nТовары:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} — {p.Price} руб. (осталось: {p.Quantity})");
        }
        Console.WriteLine($"Вы внесли: {insertedMoney} руб.");
    }

    private void InsertCoin()
    {
        Console.WriteLine("\nДоступные монеты: 1, 2, 5, 10");
        Console.Write("Введите монету: ");

        if (int.TryParse(Console.ReadLine(), out int coin))
        {
            bool exists = false;
            foreach (int c in coins)
                if (c == coin) exists = true;

            if (exists)
            {
                insertedMoney += coin;
                Console.WriteLine($"Теперь внесено: {insertedMoney}");
            }
            else
                Console.WriteLine("Такой монеты нет.");
        }
    }

    private void BuyProduct()
    {
        Console.Write("Введите id товара: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        Product product = null;
        foreach (var p in products)
            if (p.Id == id) product = p;

        if (product == null)
        {
            Console.WriteLine("Нет такого товара.");
            return;
        }

        if (product.Quantity == 0)
        {
            Console.WriteLine("Товар закончился.");
            return;
        }

        if (insertedMoney < product.Price)
        {
            Console.WriteLine("Недостаточно денег.");
            return;
        }

        product.Quantity--;
        insertedMoney -= product.Price;
        totalSales += product.Price;

        Console.WriteLine($"Куплено: {product.Name}");

        if (insertedMoney > 0)
            GiveChange();
    }

        private void GiveChange()
    {
        decimal change = insertedMoney;

        Console.Write("Сдача: ");

        while (change > 0)
        {
            if (change >= 10)
            {
                Console.Write("1x10 ");
                change -= 10;
            }
            else if (change >= 5)
            {
                Console.Write("1x5 ");
                change -= 5;
            }
            else if (change >= 2)
            {
                Console.Write("1x2 ");
                change -= 2;
            }
            else
            {
                Console.Write("1x1 ");
                change -= 1;
            }
        }

        Console.WriteLine();
        insertedMoney = 0;
    }

    private void Cancel()
    {
        if (insertedMoney > 0)
        {
            Console.WriteLine("Отмена.");
            GiveChange();
        }
        else
            Console.WriteLine("Вы ничего не внесли.");
    }

    private void AdminMenu()
    {
        Console.Write("Пароль: ");
        if (Console.ReadLine() != "admin")
        {
            Console.WriteLine("Неверный пароль");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nАдмин");
            Console.WriteLine("1. Пополнить товар");
            Console.WriteLine("2. Добавить товар");
            Console.WriteLine("3. Выручка");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                ShowProducts();
                Console.Write("Введите id: ");

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Product p = null;
                    foreach (var pr in products)
                        if (pr.Id == id) p = pr;

                    if (p != null)
                    {
                        Console.Write("Сколько добавить: ");
                        if (int.TryParse(Console.ReadLine(), out int add))
                        {
                            p.Quantity += add;
                            Console.WriteLine("Добавлено.");
                        }
                    }
                }
            }
            else if (choice == "2")
            {
                int newId = products.Count + 1;

                Console.Write("Название: ");
                string name = Console.ReadLine();

                Console.Write("Цена: ");
                decimal.TryParse(Console.ReadLine(), out decimal price);

                Console.Write("Количество: ");
                int.TryParse(Console.ReadLine(), out int qty);

                products.Add(new Product(newId, name, price, qty));
                Console.WriteLine("Добавлено.");
            }
            else if (choice == "3")
            {
                Console.WriteLine($"Выручка: {totalSales}");
            }
            else if (choice == "0")
                return;
        }
    }
}
