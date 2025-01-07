namespace NatureHelp.Exceptions.Models
{
    public class GeneralInternalException : Exception
    {
        public GeneralInternalException() :
            base("Something went wrong")
        { }

        public GeneralInternalException(string message) :
            base(message)
        { }
    }
}
