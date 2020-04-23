using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ClickOnce
{
    public abstract class ArgsBase
    {
        internal Args Default { get; set; }





        private T Validate<T>(T value, string caller)
        {


            return value;
        }
    }
}
