using System;
using System.Collections.Generic;

namespace InternetShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();
            IWarehouse iWarehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Add(iPhone12, 20);
            warehouse.Add(iPhone11, 5);

            warehouse.ShowInfo();

            Cart cart = shop.Cart();

            cart.TryAdd(iPhone12, 4, iWarehouse);
            cart.TryAdd(iPhone11, 3, iWarehouse);

            cart.ShowInfo();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9);
        }
    }

    public class Good
    {
        public Good(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public class Storage
    {
        protected readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public virtual void Add(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (count <= 0)
                throw new ArgumentOutOfRangeException();

            if (_goods.ContainsKey(good) == false)
                _goods.Add(good, count);
            else
                _goods[good] = _goods[good] + count;
        }

        public void ShowInfo()
        {
            foreach (var good in _goods)
                Console.WriteLine(good.Key.Name + " " + good.Value);
        }
    }

    public class Warehouse : Storage, IWarehouse
    {
        public bool HaveGoods(Good good, int count)
        {
            if (_goods.ContainsKey(good) == false || _goods[good] < count)
                return false;

            return true;

        }

        public void RemoveGoods(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (count <= 0)
                throw new ArgumentOutOfRangeException();

            if (_goods.ContainsKey(good) == false)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                _goods[good] = _goods[good] - count;

                if (_goods[good] == 0)
                    _goods.Remove(good);
            }
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException();

            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart();
        }
    }

    public class Cart : Storage
    {
        public void TryAdd(Good good, int count, IWarehouse warehouse)
        {
            if (good == null || warehouse == null)
                throw new ArgumentNullException();

            if (count <= 0)
                throw new ArgumentOutOfRangeException();

            if (warehouse.HaveGoods(good, count))
            {
                warehouse.RemoveGoods(good, count);
                Add(good, count);
            }
        }

        public Order Order()
        {
            Order order = new Order("https://pay.example.com/order/12345");

            return order;
        }
    }

    public class Order
    {
        public Order(string payLink)
        {
            if (payLink == null)
                throw new ArgumentNullException();

            Paylink = payLink;
        }

        public string Paylink { get; private set; }
    }

    public interface IWarehouse
    {
        bool HaveGoods(Good good, int count);
        void RemoveGoods(Good good, int count);
    }
}
