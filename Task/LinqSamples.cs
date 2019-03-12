// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Sample: Where - Task 1")]
        [Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
        public void Linq1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
		[Title("Sample: Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

        [Category("Restriction Operators")]
        [Title("HW - Task 1")]
        [Description("Return list of clietns with turnover more than x = 1000.")]
        public void Linq3()
        {
            var clients =
                from c in dataSource.Customers
                where (c.Orders.Sum(o => o.Total) > 1000)
                select c;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 2")]
        [Description("Returns list of suppliers, that are situated in the same city and country as a customer.")]
        public void Linq4()
        {
            var clients =
                from c in dataSource.Customers
                join s in dataSource.Suppliers
                on new { c.Country, c.City } equals
                   new { s.Country, s.City }
                select c;               

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 3")]
        [Description("Returns all clients, whose any order sum more than x = 15000.")]
        public void Linq5()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any(o => o.Total > 15000)
                select new { ClientName = c.CompanyName,  MaxOrderPrice = c.Orders.Max(i => i.Total)};

            //this 'max' worry me! Need to be checked!

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 4")]
        [Description("Returns customers first order date.")]
        public void Linq6()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any()
                select new { ClientName = c.CompanyName, FirstOrderDate = c.Orders.Min(o => o.OrderDate) };

            //this 'min' worry me! Need to be checked!

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 5: Order by Month")]
        [Description("Returns customers first order date order by month.")]
        public void Linq7()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any()
                let a = new { ClientName = c.CompanyName, FirstOrderDate = c.Orders.Min(o => o.OrderDate) }
                orderby a.FirstOrderDate.Month
                select a;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 5: Order by Year")]
        [Description("Returns customers first order date order by year.")]
        public void Linq8()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any()
                let a = new { ClientName = c.CompanyName, FirstOrderDate = c.Orders.Min(o => o.OrderDate) }
                orderby a.FirstOrderDate.Year
                select a;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 5: Order by Turnover")]
        [Description("Returns customers first order date order by turnover.")]
        public void Linq9()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any()
                let a = new { ClientName = c.CompanyName, Turn = c.Orders.Sum(o => o.Total) }
                orderby a.Turn descending
                select a;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 5: Order by Name")]
        [Description("Returns customers first order date order by name.")]
        public void Linq10()
        {
            var clients =
                from c in dataSource.Customers
                where c.Orders.Any()
                let a = new { ClientName = c.CompanyName}
                orderby a.ClientName
                select a;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 6: Wrong clients")]
        [Description("Returns all clients with no digital postal code or without operator code in phone number or if region is null.")]
        public void Linq11()
        {
            var clients =
                from c in dataSource.Customers
                where c.Region == null               
                   || !c.Phone.StartsWith("(")
                   || (c.PostalCode != null && new Regex(@"\D+").IsMatch(c.PostalCode))
                let a = new { ClientName = c.CompanyName, c.PostalCode, c.Phone, c.Region }
                orderby a.ClientName
                select a;

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 7: Group by categories")]
        [Description("Group products by categories, then by fact of existing product on stock, then by unit price.")]
        public void Linq12()
        {
            var clients =
                from p in dataSource.Products
                let Exist = p.UnitsInStock > 0 ? "True" : "False"
                orderby p.Category, Exist, p.UnitPrice 
                select new { p.ProductName, p.Category, Exist, p.UnitPrice };

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 8: Categories: 'cheap', 'reasonable', 'expensive'")]
        [Description("Divides products in three groups: 'cheap', 'reasonable', 'expensive'. According to unit price of product.")]
        public void Linq13()
        {
            var clients =
                from p in dataSource.Products
                let Exist = (p.UnitPrice >= 50 ? "Expensive" :
                             p.UnitPrice < 50 && p.UnitPrice > 25 ? "Reasonable" : 
                             "Cheap")
                orderby Exist, p.UnitPrice
                select new { p.ProductName, Exist, p.UnitPrice };

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 9: Average price and average intensity per client in every city")]
        [Description("Returns average revenue of each city and average order intencity.")]
        public void Linq14()
        {
            var clients =
                from c in dataSource.Customers
                orderby c.City
                group c by c.City into g
                select new
                {
                    City = g.Key,
                    AverageSum = g.Sum(gc => gc.Orders.Sum(o => o.Total)) / g.Sum(gc => gc.Orders.Count()),
                    AverageIntensity = g.Sum(gc => gc.Orders.Count()) / (double)g.Count()
                };

            foreach (var c in clients)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 10: Average statistics(Month)")]
        [Description("Returns count of orders per month average in all the years.")]
        public void Linq15()
        {
            var dateInfo = new DateTimeFormatInfo();

            var orders =
                from c in dataSource.Customers
                where c.Orders.Any()
                from o in c.Orders
                select o;

            var yearsCount = orders.Max(o => o.OrderDate.Year) - orders.Min(o => o.OrderDate.Year);

            var statistics =
                from c in dataSource.Customers
                from o in c.Orders
                group o by o.OrderDate.Month into g
                orderby g.Key
                select new { Name = dateInfo.GetMonthName(g.Key), AverageIntensity = g.Count()/(double)yearsCount };

            foreach (var c in statistics)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("HW - Task 10: Statistics(Year)")]
        [Description("Returns count of orders per year.")]
        public void Linq16()
        {
            var statistics =
                from c in dataSource.Customers
                from o in c.Orders
                group o by o.OrderDate.Year into g
                orderby g.Key
                select new { Name = g.Key, AverageIntensity = g.Count() };

            foreach (var c in statistics)
            {
                ObjectDumper.Write(c);
            }
        }
    }
}

