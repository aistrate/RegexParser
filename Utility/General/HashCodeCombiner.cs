namespace Utility.General
{
    public static class HashCodeCombiner
    {
        /// <summary>
        /// Optimized for two arguments (the most frequent case).
        /// </summary>
        public static int Combine(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }

        public static int Combine(params int[] hashCodes)
        {
            int hash = 0;

            for (int index = 0; index < hashCodes.Length; index++)
            {
                hash = (hash << 5) + hash;
                hash ^= hashCodes[index];
            }

            return hash;
        }
    }
}
