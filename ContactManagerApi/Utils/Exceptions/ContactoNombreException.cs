namespace ContactManagerApi.Utils.Exceptions
{
    public class ContactoNombreException : Exception
    {
        public string _contactName { get; set; }

        public ContactoNombreException() { }

        public ContactoNombreException(string message):base(message)
        {
            
        }

        public ContactoNombreException(string message, Exception innerException): base(message, innerException)
        {
            
        }

        public ContactoNombreException(string message, string contactName):base(message)
        {
            _contactName = contactName;
        }
    }
}
