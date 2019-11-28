using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SettingNotFoundException : Exception
{
    public SettingNotFoundException()
    {
    }

    public SettingNotFoundException(string message)
        : base(message)
    {
    }

    public SettingNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class SettingTypeNotCompatibleException : Exception
{
    public SettingTypeNotCompatibleException()
    {
    }

    public SettingTypeNotCompatibleException(string message)
        : base(message)
    {
    }

    public SettingTypeNotCompatibleException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

