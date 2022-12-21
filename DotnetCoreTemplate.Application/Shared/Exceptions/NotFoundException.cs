namespace DotnetCoreTemplate.Application.Shared.Exceptions;

public class NotFoundException : DomainException
{
    private const string ErrorMsg = "{0} '{2}' was not found.";

    public NotFoundException(Type domainType, object id)
        : base(string.Format(ErrorMsg, domainType.Name, id))
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}