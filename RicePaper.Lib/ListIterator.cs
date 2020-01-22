using System.Collections.Generic;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public abstract class ListIterator<T>
    {
        #region Protected Fields
        protected int index;
        protected List<T> currentList;
        #endregion

        #region Constructor
        public ListIterator()
        {
            currentList = LoadData();
        }
        #endregion

        #region Abstract Methods
        protected abstract List<T> LoadData();
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