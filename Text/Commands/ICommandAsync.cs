﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Text.Commands
{
    interface ICommandAsync
    {
        public Task Execute();
    }
}
