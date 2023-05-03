using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application.Torrents.Exceptions;
public class InvalidDataFormatException : Exception
{
    public InvalidDataFormatException() : base("Invalid format")
    {
        
    }

    public InvalidDataFormatException(string value) : base($"'{value}' is not in a valid format.")
    {
        
    }
}
