using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace COMMO.Server.Data
{
    public class Tools
    {
		private static DateTime _startTime;
        public static uint Uptime { get { return (uint)((DateTime.Now - _startTime).Ticks/10000); } }

        public static void Initialize()
        {
            _startTime = DateTime.Now;
        }

        public static bool ConvertLuaBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            char ch = value.ToLowerInvariant()[0];
            return ch != 'f' && ch != 'n' && ch != '0';
        }

        public static int FlagEnumToArrayPoint(uint value)
        {
            if (value == 0) return -1;

            for (int i = 0; i < 32; i++)
            {
                value >>= 1;
                if (value == 0) return i;
            }
            return -1;
        }

        public static uint AdlerChecksum(byte[] data, int index, int length)
        {
            const ushort adler = 65521;

            uint a = 1, b = 0;

            while (length > 0)
            {
                int tmp = (length > 5552) ? 5552 : length;
                length -= tmp;

                do
                {
                    a += data[index++];
                    b += a;
                } while (--tmp > 0);

                a %= adler;
                b %= adler;
            }

            return (b << 16) | a;
        }

        public static byte GetPercentage(ulong current, ulong max)
        {
            if (max == 0)
                return 0;

            byte result = (byte)((current * 100) / max);
            if (result > 100)
                return 0;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetSystemMilliseconds()
        {
            return DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;
        }
    }
}
