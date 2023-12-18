#if CSHARP_7_3_OR_NEWER

using System;

namespace Gpm.Common.ThirdParty.SharpCompress.Crypto
{
    public class KeyParameter
        : ICipherParameters
    {
        private readonly byte[] key;

        public KeyParameter(
            byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.key = (byte[])key.Clone();
        }

        public KeyParameter(
            byte[] key,
            int keyOff,
            int keyLen)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (keyOff < 0 || keyOff > key.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(keyOff));
            }
            if (keyLen < 0 || (keyOff + keyLen) > key.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(keyLen));
            }

            this.key = new byte[keyLen];
            Array.Copy(key, keyOff, this.key, 0, keyLen);
        }

        public byte[] GetKey()
        {
            return (byte[])key.Clone();
        }
    }
}

#endif