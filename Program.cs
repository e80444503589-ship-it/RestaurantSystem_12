using System;
using System.Collections.Generic;

class MenuItem
{
    public string Name;
    public int Price;
    public string Description;
}

class Client
{
    public string Name;
    public string Phone;
}

class Order
{
    public Client Client;
    public List<MenuItem> Items;
    public bool IsConfirmed;  // Подтверждён администратором
    public bool IsPaid;       // Оплачен клиентом
}

class Bill
{
    public Order Order;
    public int TotalSum;
}

class Program
{
    static List<MenuItem> menu = new List<MenuItem>();
    static List<Client> clients = new List<Client>();
    static List<Order> orders = new List<Order>();
    static List<Bill> bills = new List<Bill>();

    static void Main()
    {
        // Начальные данные меню
        menu.Add(new MenuItem { Name = "Борщ", Price = 15, Description = "Традиционный суп" });
        menu.Add(new MenuItem { Name = "Цезарь с курицей", Price = 12, Description = "Салат" });
        menu.Add(new MenuItem { Name = "Стейк рибай", Price = 40, Description = "Говядина" });
        menu.Add(new MenuItem { Name = "Тирамису", Price = 14, Description = "Десерт" });

        // Начальные клиенты
        clients.Add(new Client { Name = "Александр Иванов", Phone = "+375(44)425-98-75" });
        clients.Add(new Client { Name = "Анастасия Сидорова", Phone = "+375(29)569-65-43" });

        while (true)
        {
            Console.Clear();
            Console.WriteLine("СИСТЕМА РЕСТОРАН");
            Console.WriteLine("1 - Меню");
            Console.WriteLine("2 - Клиенты");
            Console.WriteLine("3 - Заказы");
            Console.WriteLine("4 - Счета");
            Console.WriteLine("5 - Новый клиент");
            Console.WriteLine("6 - Новый заказ (клиент)");
            Console.WriteLine("7 - Подтвердить заказ (администратор)");
            Console.WriteLine("8 - Выставить счёт (администратор)");
            Console.WriteLine("9 - Оплатить счёт (клиент)");
            Console.WriteLine("0 - Выход");
            Console.Write("> ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\nМЕНЮ:");
                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {menu[i].Name} - {menu[i].Price} руб. ({menu[i].Description})");
                }
                Wait();
            }
            else if (choice == "2")
            {
                Console.WriteLine("\nКЛИЕНТЫ:");
                for (int i = 0; i < clients.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {clients[i].Name} - {clients[i].Phone}");
                }
                Wait();
            }
            else if (choice == "3")
            {
                Console.WriteLine("\nЗАКАЗЫ:");
                if (orders.Count == 0)
                {
                    Console.WriteLine("Нет заказов.");
                }
                else
                {
                    for (int i = 0; i < orders.Count; i++)
                    {
                        string status = orders[i].IsConfirmed ? "ПОДТВЕРЖДЁН" : "НЕ ПОДТВЕРЖДЁН";
                        Console.WriteLine($"{i + 1}. Клиент: {orders[i].Client.Name} [{status}]");
                    }
                }
                Wait();
            }
            else if (choice == "4")
            {
                Console.WriteLine("\nСЧЁТА:");
                if (bills.Count == 0)
                {
                    Console.WriteLine("Нет счетов.");
                }
                else
                {
                    for (int i = 0; i < bills.Count; i++)
                    {
                        string paid = bills[i].Order.IsPaid ? "ОПЛАЧЕН" : "НЕ ОПЛАЧЕН";
                        Console.WriteLine($"{i + 1}. Заказ клиента {bills[i].Order.Client.Name} - {bills[i].TotalSum} руб. [{paid}]");
                    }
                }
                Wait();
            }
            else if (choice == "5") // Новый клиент
            {
                Client client = new Client();
                Console.Write("Имя клиента: ");
                client.Name = Console.ReadLine();
                Console.Write("Телефон: ");
                client.Phone = Console.ReadLine();
                clients.Add(client);
                Console.WriteLine("Клиент добавлен!");
                Wait();
            }
            else if (choice == "6") // Новый заказ (клиент)
            {
                if (clients.Count == 0 || menu.Count == 0)
                {
                    Console.WriteLine("Добавьте клиентов и позиции меню!");
                    Wait();
                    continue;
                }

                // Выбор клиента
                Console.WriteLine("\nВыберите клиента:");
                for (int i = 0; i < clients.Count; i++)
                    Console.WriteLine($"{i + 1}. {clients[i].Name}");
                Console.Write("Номер: ");
                int clientId = int.Parse(Console.ReadLine()) - 1;

                Order order = new Order { Client = clients[clientId], Items = new List<MenuItem>(), IsConfirmed = false, IsPaid = false };

                // Выбор блюд
                while (true)
                {
                    Console.WriteLine("\nМеню:");
                    for (int i = 0; i < menu.Count; i++)
                        Console.WriteLine($"{i + 1}. {menu[i].Name} - {menu[i].Price} руб.");
                    Console.WriteLine("0 - Завершить заказ");

                    Console.Write("Выберите блюдо: ");
                    int dishId = int.Parse(Console.ReadLine());
                    if (dishId == 0) break;
                    dishId--;

                    order.Items.Add(menu[dishId]);
                    Console.WriteLine($"{menu[dishId].Name} добавлено в заказ.");
                }

                if (order.Items.Count > 0)
                {
                    orders.Add(order);
                    Console.WriteLine("Заказ создан! Ожидает подтверждения администратора.");
                }
                else
                {
                    Console.WriteLine("Заказ пустой, не сохранён.");
                }
                Wait();
            }
            else if (choice == "7") // Подтвердить заказ (администратор)
            {
                if (orders.Count == 0)
                {
                    Console.WriteLine("Нет заказов для подтверждения!");
                    Wait();
                    continue;
                }

                Console.WriteLine("\nЗаказы для подтверждения:");
                for (int i = 0; i < orders.Count; i++)
                {
                    if (!orders[i].IsConfirmed)
                        Console.WriteLine($"{i + 1}. Клиент: {orders[i].Client.Name}");
                }
                Console.Write("Выберите заказ для подтверждения: ");
                int orderId = int.Parse(Console.ReadLine()) - 1;

                if (!orders[orderId].IsConfirmed)
                {
                    orders[orderId].IsConfirmed = true;
                    Console.WriteLine("Заказ подтверждён и отправлен на кухню!");
                }
                else
                {
                    Console.WriteLine("Этот заказ уже подтверждён.");
                }
                Wait();
            }
            else if (choice == "8") // Выставить счёт (администратор)
            {
                if (orders.Count == 0)
                {
                    Console.WriteLine("Нет заказов!");
                    Wait();
                    continue;
                }

                Console.WriteLine("\nПодтверждённые заказы без счёта:");
                int count = 0;
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].IsConfirmed && !orders[i].IsPaid)
                    {
                        Console.WriteLine($"{count + 1}. Клиент: {orders[i].Client.Name}");
                        count++;
                    }
                }
                if (count == 0)
                {
                    Console.WriteLine("Нет подходящих заказов.");
                    Wait();
                    continue;
                }

                Console.Write("Выберите заказ для выставления счёта: ");
                int selected = int.Parse(Console.ReadLine()) - 1;
                int realIndex = -1;
                count = 0;
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].IsConfirmed && !orders[i].IsPaid)
                    {
                        if (count == selected) { realIndex = i; break; }
                        count++;
                    }
                }

                int total = 0;
                foreach (var item in orders[realIndex].Items)
                    total += item.Price;

                Bill bill = new Bill { Order = orders[realIndex], TotalSum = total };
                bills.Add(bill);
                Console.WriteLine($"Счёт на {total} руб. выставлен клиенту {orders[realIndex].Client.Name}!");
                Wait();
            }
            else if (choice == "9") // Оплатить счёт (клиент)
            {
                if (bills.Count == 0)
                {
                    Console.WriteLine("Нет счетов для оплаты!");
                    Wait();
                    continue;
                }

                Console.WriteLine("\nСчета для оплаты:");
                for (int i = 0; i < bills.Count; i++)
                {
                    if (!bills[i].Order.IsPaid)
                        Console.WriteLine($"{i + 1}. Клиент: {bills[i].Order.Client.Name} - {bills[i].TotalSum} руб.");
                }
                Console.Write("Выберите счёт для оплаты: ");
                int billId = int.Parse(Console.ReadLine()) - 1;

                if (!bills[billId].Order.IsPaid)
                {
                    bills[billId].Order.IsPaid = true;
                    Console.WriteLine("Счёт оплачен! Спасибо за визит!");
                }
                else
                {
                    Console.WriteLine("Этот счёт уже оплачен.");
                }
                Wait();
            }
            else if (choice == "0")
            {
                break;
            }
        }
    }

    static void Wait()
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadLine();
    }
}
