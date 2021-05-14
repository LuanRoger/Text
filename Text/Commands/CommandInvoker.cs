using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text.Commands
{
    public class CommandInvoker
    {
        private ICommand startCommand;

        private ICommand? endCommand;

        public CommandInvoker(ICommand startCommand, ICommand endCommand = null)
        {
            this.startCommand = startCommand;
            this.endCommand = endCommand;
        }

        public void Invoker()
        {
            startCommand.Execute();

            endCommand?.Execute();
        }
    }
    public class CommandInvokerAsync
    {
        private ICommandAsync startCommandAsync;

        public CommandInvokerAsync(ICommandAsync startCommandAsync)
        {
            this.startCommandAsync = startCommandAsync;
        }

        public async Task Invoker() => await startCommandAsync.Execute();
    }
    public class QueryInvoker<T>
    {
        private Query<T> queryStartCommand;

        public QueryInvoker(Query<T> queryStartCommand)
        {
            this.queryStartCommand = queryStartCommand;
        }

        public T Invoker()
        {
            return queryStartCommand.Execute();
        }
    }
}
