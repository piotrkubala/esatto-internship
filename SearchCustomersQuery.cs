/**
* This class is used to search for customers.
* All fields are optional, if a field is null it will not be used in the search.
*/
class SearchCustomersQuery {
    public long ?min_id;
    public long ?max_id;
    public String ?name;
    public String ?vat_id;
    public DateTime ?min_creation_date;
    public DateTime ?max_creation_date;
    public String ?city;
    public String ?country;
    public int ?min_house_number;
    public int ?max_house_number;
}