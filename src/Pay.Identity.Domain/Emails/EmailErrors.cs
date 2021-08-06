using FluentResults;

namespace Pay.Identity.Domain.Emails
{
    public class RequiredError : Error
    {
        public string FieldName { get; private set; }
        
        public RequiredError(string fieldName) 
            : base($"{fieldName} is required.")
        {
            FieldName = fieldName;
            Metadata.Add("ErrorCode", 100);
        }
    }

    public class UnauthorizedError : Error
    {
        public UnauthorizedError() 
            : base("Email service can't be accessed due to invalid credentials (e.g. API Key is not valid)")
        {
            Metadata.Add("ErrorCode", 100);
        }
    }

    public class UnknownError : Error
    {
        public UnknownError() 
            : base("Unknown error")
        {
            Metadata.Add("ErrorCode", 100);
        }
    }

}