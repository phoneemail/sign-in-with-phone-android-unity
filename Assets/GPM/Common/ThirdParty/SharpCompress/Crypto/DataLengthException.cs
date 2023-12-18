#if CSHARP_7_3_OR_NEWER

using System;

namespace Gpm.Common.ThirdParty.SharpCompress.Crypto
{
    public class DataLengthException
        : CryptoException
    {
        /**
        * base constructor.
        */

        public DataLengthException()
        {
        }

        /**
         * create a DataLengthException with the given message.
         *
         * @param message the message to be carried with the exception.
         */

        public DataLengthException(
            string message)
            : base(message)
        {
        }

        public DataLengthException(
            string message,
            Exception exception)
            : base(message, exception)
        {
        }
    }
}

#endif