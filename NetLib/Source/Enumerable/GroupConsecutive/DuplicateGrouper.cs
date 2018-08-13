using System.Collections.Generic;

namespace NetLib
{
    internal class DuplicateGrouper<T>
    {
        private T CurrentKey;
        private IEnumerator<T> Itr;
        private bool More;

        public IEnumerable<IEnumerable<T>> GroupByDuplicate(IEnumerable<T> src)
        {
            using(Itr = src.GetEnumerator())
            {
                More = Itr.MoveNext();
                while (More)
                    yield return GetDuplicates();
            }
        }

        private IEnumerable<T> GetDuplicates()
        {
            CurrentKey = Itr.Current;
            while (More && CurrentKey.Equals(Itr.Current))
            {
                yield return Itr.Current;
                More = Itr.MoveNext();
            }
        }
    }
}