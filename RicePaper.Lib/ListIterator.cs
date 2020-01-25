using System;
using System.Collections.Generic;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public abstract class ListIterator<T>
    {
        #region Protected Fields
        protected readonly AppSettings settings;
        private int index;
        private int max;
        protected IList<T> currentList;
        #endregion

        #region Constructor
        public ListIterator(AppSettings settings, int startAt = 0)
        {
            this.settings = settings;

            currentList = LoadData();

            Reset(currentList.Count, startAt);
        }

        protected void Reset(int max = 0, int startAt = 0)
        {
            this.index = startAt;
            this.max = max;
        }
        #endregion

        #region Abstract Methods
        protected abstract IList<T> LoadData();
        protected abstract IList<T> LoadData(string path);
        protected abstract void PostIncrement(int preIncrement, int postIncrement);
        #endregion

        #region Public Methods
        public virtual T CurrentItem => currentList[index];

        public virtual void LoadNewList(string path)
        {
            this.currentList = LoadData(path);
            Reset(this.currentList.Count);
        }

        public void Increment()
        {
            int preIncrement = index;
            index = (index + 1) % max;
            PostIncrement(preIncrement, index);
        }
        #endregion
    }
}