using System;

namespace Mahjong.Exceptions
{

    public class BlockSortException : Exception
    {
        public BlockSortException()
        { }

        public BlockSortException(string message)
            : base(message)
        { }

        public BlockSortException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
