using System;
using System.Collections.Generic;

namespace RicePaper.Lib
{
    public abstract class ListIterator<T>
    {
        #region Static Constants
        private static Random random = new Random();
        #endregion

        #region Protected Fields
        private int index;
        private int max;
        protected IList<T> currentList;
        #endregion

        #region Properties
        public int Index { get { return index; } }
        #endregion

        #region Constructor
        public ListIterator() { }

        protected void Reset(int max = 0, int startAt = 0)
        {
            this.index = startAt;
            this.max = max;
        }
        #endregion

        #region Abstract Methods
        protected abstract IList<T> LoadData(string path);
        #endregion

        #region Public Methods
        public virtual T CurrentItem => currentList[index];

        public virtual void Load(string path, int startAt = 0)
        {
            this.currentList = LoadData(path);
            Reset(this.currentList.Count, startAt);
        }

        public int Increment(SelectionMode mode)
        {
            if (mode == SelectionMode.InOrder)
            {
                index = (index + 1) % max;
            }
            else
            {
                index = random.Next(0, currentList.Count);
            }

            return index;
        }
        #endregion
    }
}