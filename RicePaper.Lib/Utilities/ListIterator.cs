using System.Collections.Generic;

namespace RicePaper.Lib
{
    public abstract class ListIterator<T>
    {
        #region Protected Fields
        private int index;
        private int max;
        protected IList<T> currentList;
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

        public int Increment()
        {
            index = (index + 1) % max;
            return index;
        }
        #endregion
    }
}