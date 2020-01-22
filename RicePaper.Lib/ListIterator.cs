using System.Collections.Generic;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public abstract class ListIterator<T>
    {
        #region Protected Fields
        protected readonly AppSettings settings;
        protected int index;
        protected IList<T> currentList;
        #endregion

        #region Constructor
        public ListIterator(AppSettings settings, int startAt = 0)
        {
            this.settings = settings;
            this.index = startAt;
            currentList = LoadData();
        }
        #endregion

        #region Abstract Methods
        protected abstract IList<T> LoadData();
        protected abstract void PostIncrement(int preIncrement, int index);
        #endregion

        #region Public Methods
        public virtual T CurrentItem => currentList[index];

        public void Increment()
        {
            int preIncrement = index;
            index++;
            PostIncrement(preIncrement, index);
        }
        #endregion
    }
}