using System;
using System.Linq;
using Microsoft.OData.Client;
using System.ComponentModel;
using ODataDemo;

namespace ODataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //WCF api connected service URL
            var serviceUri = new Uri("https://services.odata.org/V4/OData/OData.svc/");
            var context = new DemoService(serviceUri);

            ListPeople(context);
            SearchPeople(context);
            ShowPersonDetails(context);
            ModifyPersonData(context);
        }

        // List of all customers
        static void ListPeople(DemoService context)
        {
            try
            {
                Console.WriteLine("List of all customers:");
                var customers = context.Persons.ToList();
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.ID}: {customer.Name}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // Search customers by contact name
        static void SearchPeople(DemoService context)
        {
            try
            {
                Console.WriteLine("\nEnter a name to search for:");
                var searchName = Console.ReadLine();
                var filteredCustomers = context.Persons.Where(c => c.Name.Contains(searchName)).ToList();
                Console.WriteLine($"Customers with contact name containing '{searchName}':");
                foreach (var customer in filteredCustomers)
                {
                    Console.WriteLine($"{customer.ID}: {customer.Name}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Show details of a customer
        static void ShowPersonDetails(DemoService context)
        {
            try
            {
                Console.WriteLine("\nEnter the ID of the customer to view details:");
                var customerId = int.Parse(Console.ReadLine());
                var selectedCustomer = context.Persons.Where(c => c.ID == customerId).FirstOrDefault();
                var selectedCustomerDetails = context.PersonDetails.Where(c => c.PersonID == customerId).FirstOrDefault();
                if (selectedCustomer != null && selectedCustomerDetails != null)
                {
                    Console.WriteLine($"Details of customer with ID {customerId}:");
                    Console.WriteLine($"Contact Name: {selectedCustomer.Name}");
                    Console.WriteLine($"Country: {selectedCustomerDetails.Address.Country}");
                    Console.WriteLine($"Age: {selectedCustomerDetails.Age}");
                    Console.WriteLine($"Phone: {selectedCustomerDetails.Phone}");
                }
                else
                {
                    Console.WriteLine($"Customer with ID {customerId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Modify customer's name
        static void ModifyPersonData(DemoService context)
        {
            try
            {
                Console.WriteLine("\nEnter the ID of the customer to update:");
                var customerId = int.Parse(Console.ReadLine());
                var selectedCustomer = context.Persons.Where(c => c.ID == customerId).FirstOrDefault();
                if (selectedCustomer != null)
                {
                    Console.WriteLine("Enter new contact name:");
                    var newContactName = Console.ReadLine();
                    selectedCustomer.Name = newContactName;

                    // Update customer in the context
                    context.UpdateObject(selectedCustomer);
                    context.SaveChanges();

                    Console.WriteLine($"Customer with ID {customerId} has been updated.");
                }
                else
                {
                    Console.WriteLine($"Customer with ID {customerId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
