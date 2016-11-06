// Copyright © 2016  ASM-SW
//asmeyers@outlook.com  https://github.com/asm-sw
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonorStatement
{
    interface ISubForm
    {
        bool CanExit(out string errorMsg);
    }
}
