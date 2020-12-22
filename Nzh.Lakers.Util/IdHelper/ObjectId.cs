using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Nzh.Lakers.Util.IdHelper
{
    internal struct ObjectId : IComparable<ObjectId>, IEquatable<ObjectId>
    {
        private static readonly DateTime __unixEpoch;
        private static readonly long __dateTimeMaxValueMillisecondsSinceEpoch;
        private static readonly long __dateTimeMinValueMillisecondsSinceEpoch;
        private static ObjectId __emptyInstance = default(ObjectId);
        private static int __staticMachine;
        private static short __staticPid;
        private static int __staticIncrement;
        private static uint[] _lookup32 = Enumerable.Range(0, 256).Select(i => {
            string s = i.ToString("x2");
            return ((uint)s[0]) + ((uint)s[1] << 16);
        }).ToArray();

        private int _timestamp;
        private int _machine;
        private short _pid;
        private int _increment;

        static ObjectId()
        {
            __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            __dateTimeMaxValueMillisecondsSinceEpoch = (DateTime.MaxValue - __unixEpoch).Ticks / 10000;
            __dateTimeMinValueMillisecondsSinceEpoch = (DateTime.MinValue - __unixEpoch).Ticks / 10000;
            __staticMachine = GetMachineHash();
            __staticIncrement = (new System.Random()).Next();
            __staticPid = (short)GetCurrentProcessId();
        }

        public ObjectId(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            Unpack(bytes, out _timestamp, out _machine, out _pid, out _increment);
        }

        public ObjectId(DateTime timestamp, int machine, short pid, int increment)
            : this(GetTimestampFromDateTime(timestamp), machine, pid, increment)
        {
        }

        public ObjectId(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            _timestamp = timestamp;
            _machine = machine;
            _pid = pid;
            _increment = increment;
        }

        public ObjectId(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Unpack(ParseHexString(value), out _timestamp, out _machine, out _pid, out _increment);
        }

        public static ObjectId Empty
        {
            get { return __emptyInstance; }
        }

        public int Timestamp
        {
            get { return _timestamp; }
        }

        public int Machine
        {
            get { return _machine; }
        }

        public short Pid
        {
            get { return _pid; }
        }

        public int Increment
        {
            get { return _increment; }
        }

        public DateTime CreationTime
        {
            get { return __unixEpoch.AddSeconds(_timestamp); }
        }

        public static bool operator <(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public static bool operator ==(ObjectId lhs, ObjectId rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ObjectId lhs, ObjectId rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >=(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator >(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static ObjectId GenerateNewId()
        {
            return GenerateNewId(GetTimestampFromDateTime(DateTime.UtcNow));
        }

        public static ObjectId GenerateNewId(DateTime timestamp)
        {
            return GenerateNewId(GetTimestampFromDateTime(timestamp));
        }

        public static ObjectId GenerateNewId(int timestamp)
        {
            int increment = Interlocked.Increment(ref __staticIncrement) & 0x00ffffff;
            return new ObjectId(timestamp, __staticMachine, __staticPid, increment);
        }

        public static string GenerateNewStringId()
        {
            return GenerateNewId().ToString();
        }

        public static byte[] Pack(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            byte[] bytes = new byte[12];
            bytes[0] = (byte)(timestamp >> 24);
            bytes[1] = (byte)(timestamp >> 16);
            bytes[2] = (byte)(timestamp >> 8);
            bytes[3] = (byte)(timestamp);
            bytes[4] = (byte)(machine >> 16);
            bytes[5] = (byte)(machine >> 8);
            bytes[6] = (byte)(machine);
            bytes[7] = (byte)(pid >> 8);
            bytes[8] = (byte)(pid);
            bytes[9] = (byte)(increment >> 16);
            bytes[10] = (byte)(increment >> 8);
            bytes[11] = (byte)(increment);
            return bytes;
        }

        public static ObjectId Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length != 24)
            {
                throw new ArgumentOutOfRangeException("s", "ObjectId string value must be 24 characters.");
            }
            return new ObjectId(ParseHexString(s));
        }

        public static void Unpack(byte[] bytes, out int timestamp, out int machine, out short pid, out int increment)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (bytes.Length != 12)
            {
                throw new ArgumentOutOfRangeException("bytes", "Byte array must be 12 bytes long.");
            }
            timestamp = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3];
            machine = (bytes[4] << 16) + (bytes[5] << 8) + bytes[6];
            pid = (short)((bytes[7] << 8) + bytes[8]);
            increment = (bytes[9] << 16) + (bytes[10] << 8) + bytes[11];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        private static int GetMachineHash()
        {
            var hostName = Environment.MachineName; // use instead of Dns.HostName so it will work offline
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(hostName));
            return (hash[0] << 16) + (hash[1] << 8) + hash[2]; // use first 3 bytes of hash
        }

        private static int GetTimestampFromDateTime(DateTime timestamp)
        {
            return (int)Math.Floor((ToUniversalTime(timestamp) - __unixEpoch).TotalSeconds);
        }

        public int CompareTo(ObjectId other)
        {
            int r = _timestamp.CompareTo(other._timestamp);
            if (r != 0) { return r; }
            r = _machine.CompareTo(other._machine);
            if (r != 0) { return r; }
            r = _pid.CompareTo(other._pid);
            if (r != 0) { return r; }
            return _increment.CompareTo(other._increment);
        }

        public bool Equals(ObjectId rhs)
        {
            return
                _timestamp == rhs._timestamp &&
                _machine == rhs._machine &&
                _pid == rhs._pid &&
                _increment == rhs._increment;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectId)
            {
                return Equals((ObjectId)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = 37 * hash + _timestamp.GetHashCode();
            hash = 37 * hash + _machine.GetHashCode();
            hash = 37 * hash + _pid.GetHashCode();
            hash = 37 * hash + _increment.GetHashCode();
            return hash;
        }

        public byte[] ToByteArray()
        {
            return Pack(_timestamp, _machine, _pid, _increment);
        }

        public override string ToString()
        {
            return ToHexString(ToByteArray());
        }

        public static byte[] ParseHexString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (s.Length % 2 == 1)
            {
                throw new Exception("The binary key cannot have an odd number of digits");
            }

            byte[] arr = new byte[s.Length >> 1];

            for (int i = 0; i < s.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(s[i << 1]) << 4) + (GetHexVal(s[(i << 1) + 1])));
            }

            return arr;
        }
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = _lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        public static long ToMillisecondsSinceEpoch(DateTime dateTime)
        {
            var utcDateTime = ToUniversalTime(dateTime);
            return (utcDateTime - __unixEpoch).Ticks / 10000;
        }

        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
