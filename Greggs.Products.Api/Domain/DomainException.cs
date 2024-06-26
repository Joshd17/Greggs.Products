using System;

namespace Greggs.Products.Api.Domain;

public class DomainException : Exception
{
    public DomainException(string message):base(message)
    {
        
    }
}
