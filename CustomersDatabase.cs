using System.Text.Json;

class CustomerDatabase {
    /**
    * Dictionary of all customers in the database
    * Key: Customer ID
    */
    private Dictionary<long, Customer> customers_dictionary = new Dictionary<long, Customer>();
    private long next_customer_id = 0;

    private long GenNextCustomerId() {
        // Generate next customer id
        return next_customer_id++;
    }

    /**
    * Add a customer to the database
    * @param customer The customer to add to the database, copy of the customer will be added
    */
    public void AddCustomer(Customer customer) {
        Customer customer_copy = (Customer) customer.Clone();

        if (customer_copy.id is null) {
            customer_copy.id = GenNextCustomerId();
        } else {
            if (customers_dictionary.ContainsKey((long) customer_copy.id)) {
                throw new BadIdException("Customer with id " + customer_copy.id + " already exists");
            }
        }

        customers_dictionary.Add((long) customer_copy.id, customer_copy);
    }

    /**
    * Edit a customer in the database, all not null fields will be replaced
    * @param new_customer_data The new data to replace the old data with, object to replace will be found by id
    */
    public void EditCustomer(Customer new_customer_data) {
        // Edit customer in database

        if (new_customer_data.id is null) {
            throw new BadIdException("Customer id is null");
        }

        if (!customers_dictionary.ContainsKey((long) new_customer_data.id)) {
            throw new BadIdException("Customer with id " + new_customer_data.id + " does not exist");
        }

        Customer customer_to_edit = customers_dictionary[(long) new_customer_data.id];

        // it copies all necessary fields from new_customer_data to customer_to_edit
        customer_to_edit.ReplaceAllNotNull(new_customer_data);
    }

    /**
    * Delete a customer from the database
    * @param customer_id The ID of the customer to delete
    */
    public void DeleteCustomer(long customer_id) {
        // Delete customer from database

        if (!customers_dictionary.ContainsKey(customer_id)) {
            throw new BadIdException("Customer with id " + customer_id + " does not exist");
        }

        customers_dictionary.Remove(customer_id);
    }

    /**
    * Get a customer from the database
    * @param customer_id The ID of the customer to get
    * @return The customer with the given ID
    */
    public Customer GetCustomer(long customer_id) {
        // Get customer from database

        if (!customers_dictionary.ContainsKey(customer_id)) {
            throw new BadIdException("Customer with id " + customer_id + " does not exist");
        }

        return customers_dictionary[customer_id];
    }

    /**
    * Get list of all customers ids in the database given a search query object
    * @param search_query The search query object to search the database with
    * @return List of all customers ids in the database given a search query object
    */
    public List<long> GetCustomerIds(SearchCustomersQuery search_query) {
        // Get list of all customers ids in the database given a search query object
        var query_result = 
            from customer in customers_dictionary.Values
                where customer.id is not null
                && (search_query.min_id is null || customer.id >= search_query.min_id)
                && (search_query.max_id is null || customer.id <= search_query.max_id)
                && (search_query.name is null || customer.name == search_query.name)
                && (search_query.vat_id is null || customer.vat_id == search_query.vat_id)
                && (search_query.min_creation_date is null || customer.creation_date >= search_query.min_creation_date)
                && (search_query.max_creation_date is null || customer.creation_date <= search_query.max_creation_date)
                && (search_query.street is null || customer.address.street == search_query.street)
                && (search_query.city is null || customer.address.city == search_query.city)
                && (search_query.country is null || customer.address.country == search_query.country)
                && (search_query.min_house_number is null || customer.address.house_number >= search_query.min_house_number)
                && (search_query.max_house_number is null || customer.address.house_number <= search_query.max_house_number)
            select (customer.id is null ? -1 : (long) customer.id); // there should be no null ids in the select return becouse of the where clause
            
        return query_result.ToList();
    }

    /**
    * Get list of all customers ids in the database
    * @return List of all customers ids in the database
    */
    public List<long> GetAllCustomerIds() {
        // Get list of all customers ids in the database
        List<long> customer_ids = new List<long>();

        foreach (var customer in customers_dictionary.Values) {
            if (customer.id is null) {
                throw new DatabaseInternalErrorException("Customer id is null");
            }

            customer_ids.Add((long) customer.id);
        }

        return customer_ids;
    }

    /**
    * Clear all entries in the database
    */
    public void Clear() {
        customers_dictionary.Clear();
        next_customer_id = 0;
    }

    /**
    * Save database to file
    * @param path The path to the file to save the database to
    */
    public void SaveToFile(String path) {
        // Save database to file
        StreamWriter ?file = null;
        try {
            file = new StreamWriter(path);
        } catch (Exception) {
            throw new DatabaseInternalErrorException("Could not open file " + path + " for writing");
        }

        foreach (var customer in customers_dictionary.Values) {
            file.WriteLine(JsonSerializer.Serialize(customer));
        }

        file.Close();
    }

    /**
    * Load database from file
    * @param path The path to the file to load the database from
    */
    public void LoadFromFile(String path) {
        StreamReader ?file = null;
        try {
            file = new StreamReader(path);
        } catch (Exception) {
            throw new DatabaseInternalErrorException("Could not open file " + path + " for reading");
        }

        int line_number = 0;
        while (!file.EndOfStream) {
            line_number++;

            String ?line = file.ReadLine();

            if (line is null) {
                throw new DatabaseInternalErrorException("Could not read line from file " + path);
            }

            try {
                Customer ?customer = JsonSerializer.Deserialize<Customer>(line);
                
                if (customer is null) {
                    throw new DatabaseInternalErrorException($"Could not deserialize customer from line ({line_number}) " + line);
                }

                if (customer.id is null || !customers_dictionary.ContainsKey((long) customer.id)) {
                    AddCustomer(customer);
                } else {
                    EditCustomer(customer);
                }
            } catch (Exception) {
                throw new DatabaseInternalErrorException($"Could not deserialize customer from line ({line_number}) " + line);
            }            
        }

        file.Close();
    }
}