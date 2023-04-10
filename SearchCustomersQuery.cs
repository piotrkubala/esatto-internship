/**
* This class is used to search for customers.
* All fields are optional, if a field is null it will not be used in the search.
*/
class SearchCustomersQuery {
    public long ?min_id = null;
    public long ?max_id = null;
    public String ?name = null;
    public String ?vat_id = null;
    public DateTime ?min_creation_date = null;
    public DateTime ?max_creation_date = null;
    public String ?street = null;
    public String ?city = null;
    public String ?country = null;
    public int ?min_house_number = null;
    public int ?max_house_number = null;
}