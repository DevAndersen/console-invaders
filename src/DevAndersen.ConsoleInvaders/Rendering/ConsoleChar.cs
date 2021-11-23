using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DevAndersen.ConsoleInvaders.Rendering
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct ConsoleChar
    {
        private const char e = (char)27;

        [FieldOffset(0)]
        public char character;



        [FieldOffset(2)]
        public int foreground;

        [FieldOffset(2)]
        public byte foregroundB;

        [FieldOffset(3)]
        public byte foregroundG;

        [FieldOffset(4)]
        public byte foregroundR;



        [FieldOffset(6)]
        public int background;

        [FieldOffset(6)]
        public byte backgroundB;

        [FieldOffset(7)]
        public byte backgroundG;

        [FieldOffset(8)]
        public byte backgroundR;



        [FieldOffset(9)]
        public ConsoleCharOptions options;

        public ConsoleChar(char character, int foreground = 0xeeeeee, ConsoleCharOptions options = ConsoleCharOptions.None) : this()
        {
            this.character = character;
            this.foreground = foreground;
            this.options = options;
        }

        public ConsoleChar(char character, int foreground, int background, ConsoleCharOptions options = ConsoleCharOptions.Background) : this(character, foreground, options)
        {
            this.background = background;
        }

        public void SetForegroundColor(int foreground)
        {
            this.foreground = foreground;
        }

        public void SetBackgroundColor(int background)
        {
            this.background = background;
            options |= ConsoleCharOptions.Background;
        }

        public void AppendToStringBuilder(StringBuilder sb)
        {
            sb.Append($"{e}[38;2;{foregroundR};{foregroundG};{foregroundB}m");
            if (options.HasFlag(ConsoleCharOptions.Background))
            {
                sb.Append($"{e}[48;2;{backgroundR};{backgroundG};{backgroundB}m");
            }
            var outChar = character == (char)0 ? ' ' : character;
            sb.Append($"{outChar}{e}[m");
        }
    }
}
