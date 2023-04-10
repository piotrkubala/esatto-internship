using System.Reflection.Metadata;
class MainClass {
    public static void AddNewCustomerPrompt(CustomerDatabase database) {
        Console.WriteLine("Enter customer name: ");
        String ?name = Console.ReadLine();
        Console.WriteLine("Enter customer VAT ID: ");
        String ?vat_id = Console.ReadLine();

        DateTime creation_date = DateTime.Now;

        Console.WriteLine("Enter customer address: ");
        CustomerAddress address = new CustomerAddress();

        Console.WriteLine("Street: ");
        address.street = Console.ReadLine();
        Console.WriteLine("City: ");
        address.city = Console.ReadLine();
        Console.WriteLine("Country: ");
        address.country = Console.ReadLine();
        Console.WriteLine("House number: ");
        String ?house_number = Console.ReadLine();

        if (house_number == null) {
            throw new InputException("House number cannot be empty");
        }

        try {
            address.house_number = int.Parse(house_number);
        } catch (Exception) {
            throw new InputException("House number must be a number");
        }

        Customer new_customer = new Customer(name, vat_id, creation_date, address);

        database.AddCustomer(new_customer);
    }

    public static void EditCustomerPrompt(CustomerDatabase database) {

    }

    public static void DeleteCustomerPrompt(CustomerDatabase database) {
        Console.WriteLine("Enter customer id to delete: ");
        String ?id = Console.ReadLine();

        if (id == null) {
            throw new InputException("Id cannot be empty");
        }

        try {
            long id_long = long.Parse(id);
            database.DeleteCustomer(id_long);
        } catch (FormatException) {
            throw new InputException("Id must be a number");
        } catch (CustomerDatabaseException) {
            throw new InputException("Customer with id " + id + " does not exist");
        }
    }

    public static void SearchCustomersPrompt(CustomerDatabase database) {

    }

    public static void ListAllCustomers(CustomerDatabase database) {
        foreach (long customerId in database.GetAllCustomerIds()) {
            Console.WriteLine(database.GetCustomer(customerId));
        }
    }

    public static void Main(String[] args) {
        CustomerDatabase database = new CustomerDatabase();

        bool should_quit = false;

        while(should_quit) {
            Console.WriteLine("Customers database");
            Console.WriteLine("[a] Add customer");
            Console.WriteLine("[e] Edit customer");
            Console.WriteLine("[d] Delete customer");
            Console.WriteLine("[s] Search customers");
            Console.WriteLine("[l] List all customers");
            Console.WriteLine("[v] Save database to file");
            Console.WriteLine("[r] Load database from file");
            Console.WriteLine("[q] Quit");

            char input_key = Console.ReadKey().KeyChar;

            try {
                switch (input_key) {
                case 'a':
                    AddNewCustomerPrompt(database);
                    break;
                case 'e':
                    EditCustomerPrompt(database);
                    break;
                case 'd':
                    DeleteCustomerPrompt(database);
                    break;
                case 's':
                    SearchCustomersPrompt(database);
                    break;
                case 'l':
                    ListAllCustomers(database);
                    break;
                case 'v':
                    break;
                case 'r':
                    break;
                case 'q':
                    should_quit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
                }
            } catch (BadIdException e) {
                Console.WriteLine(e.Message);
            } catch (Exception e) {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }
}