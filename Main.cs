using System.Reflection.Metadata;
class MainClass {
    private static void AddNewCustomerPrompt(CustomerDatabase database) {
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

    private static void EditCustomerPrompt(CustomerDatabase database) {
        long customer_id = 0;

        Console.WriteLine("Enter customer id to edit: ");
        String ?id = Console.ReadLine();

        if (id == null) {
            throw new InputException("Id cannot be empty");
        }

        try {
            customer_id = long.Parse(id);
        } catch (FormatException) {
            throw new InputException("Id must be a number");
        }

        Customer customer = database.GetCustomer(customer_id);

        char input_key = ' ';

        do {
            Console.WriteLine("Edit customer menu");
            Console.WriteLine("[n] Name");
            Console.WriteLine("[v] VAT ID");
            Console.WriteLine("[o] Country");
            Console.WriteLine("[c] City");
            Console.WriteLine("[s] Street");
            Console.WriteLine("[h] House number");
            Console.WriteLine("[q] Quit and save customer");

            input_key = Console.ReadKey().KeyChar;

            switch (input_key) {
            case 'n':
                Console.WriteLine("Enter new name: ");
                customer.name = Console.ReadLine();
                break;
            case 'v':
                Console.WriteLine("Enter new VAT ID: ");
                customer.vat_id = Console.ReadLine();
                break;
            case 'o':
                Console.WriteLine("Enter new country: ");
                customer.address.country = Console.ReadLine();
                break;
            case 'c':
                Console.WriteLine("Enter new city: ");
                customer.address.city = Console.ReadLine();
                break;
            case 's':
                Console.WriteLine("Enter new street: ");
                customer.address.street = Console.ReadLine();
                break;
            case 'h':
                Console.WriteLine("Enter new house number: ");
                String ?house_number = Console.ReadLine();

                if (house_number == null) {
                    throw new InputException("House number cannot be empty");
                }

                try {
                    customer.address.house_number = int.Parse(house_number);
                } catch (Exception) {
                    throw new InputException("House number must be a number");
                }
                break;
            case 'q':
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
            }
        } while (input_key != 'q');
        
        database.EditCustomer(customer);
    }

    private static void DeleteCustomerPrompt(CustomerDatabase database) {
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

    private static void SearchCustomersPrompt(CustomerDatabase database) {
        SearchCustomersQuery query = new SearchCustomersQuery();

        char input_key = ' ';

        do {
            Console.WriteLine("Search customers menu");
            Console.WriteLine("[i] Id");
            Console.WriteLine("[n] Name");
            Console.WriteLine("[v] VAT ID");
            Console.WriteLine("[d] Creation date");
            Console.WriteLine("[a] Address");
            Console.WriteLine("[q] Quit and search");

            input_key = Console.ReadKey().KeyChar;

            switch (input_key) {
            case 'i':
                Console.WriteLine("Id search change");
                Console.WriteLine("[i] Exact id");
                Console.WriteLine("[m] Min id");
                Console.WriteLine("[x] Max id");

                char id_search_key = Console.ReadKey().KeyChar;

                Console.WriteLine("Enter id: ");
                String ?input_id = Console.ReadLine();

                if (input_id == null) {
                    throw new InputException("Id cannot be empty");
                }

                try {
                    long id = long.Parse(input_id);

                    switch (id_search_key) {
                    case 'i':
                        query.min_id = query.max_id = id;
                        break;
                    case 'm':
                        query.min_id = id;
                        break;
                    case 'x':
                        query.max_id = id;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                    }
                } catch (FormatException) {
                    throw new InputException("Id must be a number");
                }

                break;
            case 'n':
                Console.WriteLine("Enter name: ");
                query.name = Console.ReadLine();
                break;
            case 'v':
                Console.WriteLine("Enter VAT ID: ");
                query.vat_id = Console.ReadLine();
                break;
            case 'd':
                Console.WriteLine("Creation date search change");
                Console.WriteLine("[i] Exact date");
                Console.WriteLine("[m] Min date");
                Console.WriteLine("[x] Max date");

                char date_search_key = Console.ReadKey().KeyChar;

                Console.WriteLine("Enter date: ");
                String ?input_date = Console.ReadLine();

                if (input_date == null) {
                    throw new InputException("Date cannot be empty");
                }

                try {
                    DateTime date = DateTime.Parse(input_date);

                    switch (date_search_key) {
                    case 'i':
                        query.min_creation_date = query.max_creation_date = date;
                        break;
                    case 'm':
                        query.min_creation_date = date;
                        break;
                    case 'x':
                        query.max_creation_date = date;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                    }
                } catch (FormatException) {
                    throw new InputException("Date must be in format yyyy-mm-dd");
                }

                break;
            case 'a':
                Console.WriteLine("Address search change");
                Console.WriteLine("[c] City");
                Console.WriteLine("[s] Street");
                Console.WriteLine("[o] Country");
                Console.WriteLine("[h] House number");

                char address_search_key = Console.ReadKey().KeyChar;

                String ?input_address = Console.ReadLine();

                if (input_address == null) {
                    throw new InputException("Address cannot be empty");
                }

                if (address_search_key != 'h') {
                    switch (address_search_key) {
                        case 'c':
                        query.city = input_address;
                        break;
                    case 's':
                        query.street = input_address;
                        break;
                    case 'o':
                        query.country = input_address;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                    }
                } else {
                    Console.WriteLine("House number search change");
                    Console.WriteLine("[i] Exact house number");
                    Console.WriteLine("[m] Min house number");
                    Console.WriteLine("[x] Max house number");

                    char house_number_search_key = Console.ReadKey().KeyChar;

                    String ?input_house_number = Console.ReadLine();

                    if (input_house_number == null) {
                        throw new InputException("House number cannot be empty");
                    }

                    try {
                        int house_number = int.Parse(input_house_number);

                        switch (house_number_search_key) {
                        case 'i':
                            query.min_house_number = query.max_house_number = house_number;
                            break;
                        case 'm':
                            query.min_house_number = house_number;
                            break;
                        case 'x':
                            query.max_house_number = house_number;
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                        }
                    } catch (FormatException) {
                        throw new InputException("House number must be a number");
                    }
                } 
                
                break;
            case 'q':
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
            }
        } while (input_key != 'q');

        foreach (long customerId in database.GetCustomerIds(query)) {
            Console.WriteLine(database.GetCustomer(customerId));
        }
    }

    private static void ListAllCustomers(CustomerDatabase database) {
        foreach (long customerId in database.GetAllCustomerIds()) {
            Console.WriteLine(database.GetCustomer(customerId));
        }
    }

    private static void SaveDatabaseToFile(CustomerDatabase database) {
        Console.WriteLine("Enter file name: ");
        String ?file_name = Console.ReadLine();

        if (file_name == null) {
            throw new InputException("File name cannot be empty");
        }

        database.SaveToFile(file_name);
    }

    private static void LoadDatabaseFromFile(CustomerDatabase database) {
        Console.WriteLine("Enter file name: ");
        String ?file_name = Console.ReadLine();

        if (file_name == null) {
            throw new InputException("File name cannot be empty");
        }

        Console.WriteLine("Would you like to clear the database before loading? [y/n]");
        char clear_database = Console.ReadKey().KeyChar;

        if (clear_database == 'y') {
            database.Clear();
        }

        database.LoadFromFile(file_name);
    }

    public static void Main(String[] args) {
        CustomerDatabase database = new CustomerDatabase();

        char input_key = ' ';

        do {
            Console.WriteLine("Customers database main menu");
            Console.WriteLine("[a] Add customer");
            Console.WriteLine("[e] Edit customer");
            Console.WriteLine("[d] Delete customer");
            Console.WriteLine("[s] Search customers");
            Console.WriteLine("[l] List all customers");
            Console.WriteLine("[v] Save database to file");
            Console.WriteLine("[r] Load database from file");
            Console.WriteLine("[c] Clear database");
            Console.WriteLine("[q] Quit");

            input_key = Console.ReadKey().KeyChar;

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
                    SaveDatabaseToFile(database);
                    break;
                case 'r':
                    LoadDatabaseFromFile(database);
                    break;
                case 'c':
                    database.Clear();
                    break;
                case 'q':
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
        } while(input_key != 'q');
    }
}