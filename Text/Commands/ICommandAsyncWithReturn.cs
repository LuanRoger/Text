using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    interface ICommandAsyncWithReturn<T> : ICommandAsync
    {
        T Result {get;}
    }
}
