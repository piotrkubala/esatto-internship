class Customer: ICloneable {
    public long ?id {get; set;}

    public String ?name {get; set;}
    public String ?vat_id {get; set;}
    public DateTime ?creation_date {get; set;}
    public CustomerAddress address;

    public CustomerAddress GetSetAddress {
        get {
            return address;
        }
        set {
            address = value;
        }
    }

    public Customer(String ?name, String ?vat_id, DateTime ?creation_date, CustomerAddress address) {
        this.name = name;
        this.vat_id = vat_id;
        this.creation_date = creation_date;
        this.address = address;
    }

    public Customer() {
        this.name = null;
        this.vat_id = null;
        this.creation_date = null;
        this.address = new CustomerAddress();
    }

    public object Clone() {
        return new Customer(name, vat_id, creation_date, address);
    }

    /**
    * Replace all not null fields in this object with the corresponding fields in the given object except for the id
    * @param new_customer_data The object to replace the fields with
    */
    public void ReplaceAllNotNull(Customer new_customer_data) {
        if (new_customer_data.name != null) {
            this.name = new_customer_data.name;
        }
        if (new_customer_data.vat_id != null) {
            this.vat_id = new_customer_data.vat_id;
        }
        if (new_customer_data.creation_date != null) {
            this.creation_date = new_customer_data.creation_date;
        }

        CustomerAddress new_address = (CustomerAddress) new_customer_data.address;

        if (new_address.city != null) {
            this.address.city = new_address.city;
        }
        if (new_address.country != null) {
            this.address.country = new_address.country;
        }
        if (new_address.house_number != null) {
            this.address.house_number = new_address.house_number;
        }
        if (new_address.street != null) {
            this.address.street = new_address.street;
        }
    
    }

    public override String ToString() {
        return $"Customer({id}, {name}, {vat_id}, {creation_date}, Address({address.city}, {address.country}, {address.house_number}, {address.street}))";
    }
}