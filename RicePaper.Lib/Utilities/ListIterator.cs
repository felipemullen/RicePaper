using System;
using System.Collections.Generic;
using System.Linq;

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
        private string currentPath;
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
        public virtual T CurrentItem => currentList.Count > 0 ? currentList[index] : default;

        public virtual void Load(string path, int startAt = 0)
        {
            this.currentPath = path;
            this.currentList = LoadData(path);
            Reset(this.currentList.Count, startAt);
        }

        public virtual void Reload()
        {
            if (string.IsNullOrEmpty(currentPath) == false)
            {
                this.currentList = LoadData(currentPath);
                Reset(this.currentList.Count, this.index);
            }
        }

        public int Increment(SelectionMode mode)
        {
            // A folder with no files
            if (max == 0)
            {
                index = 0;
            }
            else if (mode == SelectionMode.InOrder)
            {
                index = (index + 1) % max;
            }
            else
            {
                index = random.Next(0, currentList.Count);
            }

            // Prevent index out of bounds
            if (max > 0)
                index = Math.Clamp(index, 0, max - 1);

            return index;
        }
        #endregion
    }
}