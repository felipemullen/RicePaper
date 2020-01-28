using System.Collections.Generic;

namespace RicePaper.Lib.Utilities
{
    /// <summary>
    /// Very quick naive implementation of a cache.
    /// Has a max size and removes the oldest additions
    /// once it goes over size
    /// </summary>
    public class SimpleCache<K, T>
    {
        #region Private Fields
        private int maxSize;
        private readonly Dictionary<K, T> entries;
        private readonly Queue<KeyValuePair<K, T>> keyQueue;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor with optional size parameter. A value of -1
        /// signifies no limit to the cache
        /// </summary>
        public SimpleCache(int maxSize = -1)
        {
            this.maxSize = maxSize;
            entries = new Dictionary<K, T>();
            keyQueue = new Queue<KeyValuePair<K, T>>();
        }
        #endregion

        #region Public Methods
        public bool Contains(K key)
        {
            return entries.ContainsKey(key);
        }

        public T Get(K key)
        {
            T value = entries[key];
            return value;
        }

        public void Add(K key, T value)
        {
            Cleanup();

            if (entries.ContainsKey(key))
            {
                entries[key] = value;
            }
            else
            {
                var pair = new KeyValuePair<K, T>(key, value);
                entries.Add(key, value);
                keyQueue.Enqueue(pair);
            }
        }

        public void Clear()
        {
            entries.Clear();
            keyQueue.Clear();
        }
        #endregion

        #region Private Methods
        private void Cleanup()
        {
            if (maxSize == -1)
                return;

            int sizeDiff = entries.Count - maxSize;
            if (sizeDiff > 0)
            {
                for (int i = 0; i < sizeDiff; i++)
                {
                    var entry = keyQueue.Dequeue();
                    entries.Remove(entry.Key);
                }
            }
        }
        #endregion
    }
}
