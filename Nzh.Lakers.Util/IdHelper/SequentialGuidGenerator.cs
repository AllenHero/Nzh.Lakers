using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Nzh.Lakers.Util.IdHelper
{
    public class SequentialGuidGenerator
    {
        public static SequentialGuidGenerator Instance { get; } = new SequentialGuidGenerator();

        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public SequentialGuidDatabaseType DatabaseType { get; set; }

        private SequentialGuidGenerator()
        {
            DatabaseType = SequentialGuidDatabaseType.SqlServer;
        }

        public Guid Create()
        {
            return Create(DatabaseType);
        }

        public Guid Create(SequentialGuidDatabaseType databaseType)
        {
            switch (databaseType)
            {
                case SequentialGuidDatabaseType.SqlServer:
                    return Create(SequentialGuidType.SequentialAtEnd);
                case SequentialGuidDatabaseType.Oracle:
                    return Create(SequentialGuidType.SequentialAsBinary);
                case SequentialGuidDatabaseType.MySql:
                    return Create(SequentialGuidType.SequentialAsString);
                case SequentialGuidDatabaseType.PostgreSql:
                    return Create(SequentialGuidType.SequentialAsString);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Guid Create(SequentialGuidType guidType)
        {
            var randomBytes = new byte[10];
            Rng.Locking(r => r.GetBytes(randomBytes));
            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            byte[] guidBytes = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;
                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }
            return new Guid(guidBytes);
        }

        public enum SequentialGuidDatabaseType
        {
            SqlServer,
            Oracle,
            MySql,
            PostgreSql,
        }

        public enum SequentialGuidType
        {
            SequentialAsString,
            SequentialAsBinary,
            SequentialAtEnd
        }
    }
}
