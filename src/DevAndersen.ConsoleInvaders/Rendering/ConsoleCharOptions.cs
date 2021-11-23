using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Rendering
{
    [Flags]
    internal enum ConsoleCharOptions : byte
    {
        None =          0b_0000_0000,
        Background =    0b_0000_0001
    }
}
