using System;

namespace EventApp.App.Exceptions;

public class NotFoundException : Exception
{

    public NotFoundException(string message) : base(message)
    {
        
    }

}