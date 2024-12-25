// Copyright © 2016-2024 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;


namespace DonorStatement
{
    interface ISubForm
    {
        bool CanExit(out string errorMsg);
    }
}
