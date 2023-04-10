class BadIdException : CustomerDatabaseException
{
    public BadIdException(string message) : base(message) {
    }
}