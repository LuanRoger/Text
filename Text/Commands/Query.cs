using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    public interface Query<TResult>
    {
        TResult Execute();
    }
}
