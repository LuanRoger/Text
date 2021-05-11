using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    interface ICommandWithReturn<T> : ICommand
    {
        T Result { get; }
    }
}
