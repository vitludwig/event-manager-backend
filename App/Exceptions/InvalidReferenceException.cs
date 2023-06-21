using System;

namespace EventApp.App.Exceptions;

public class InvalidReferenceException : Exception
{
    public InvalidReferenceException(string message) : base(message)
    {
    }
}