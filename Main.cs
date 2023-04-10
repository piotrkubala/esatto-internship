using System.Reflection.Metadata;
class MainClass {
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
            Console.WriteLine("[q] Quit");

            char input_key = Console.ReadKey().KeyChar;

            try {
                switch (input_key) {
                case 'a':
                    break;
                case 'e':
                    break;
                case 'd':
                    break;
                case 's':
                    break;
                case 'l':
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