namespace NetLib
{
    using System;
    using System.Collections.Generic;

    internal class DuplicateGrouper<T, Tkey>
    {
        private Func<T, Tkey> _keySelector;
        private Tkey CurrentKey;
        private IEnumerator<T> Itr;
        private bool More;

        public IEnumerable<IEnumerable<T>> GroupByDuplicate(IEnumerable<T> src, Func<T, Tkey> keySelector)
        {
            _keySelector = keySelector;
            using(Itr = src.GetEnumerator())
            {
                More = Itr.MoveNext();
                while (More)
                    yield return GetDuplicates();
            }
        }

        private IEnumerable<T> GetDuplicates()
        {
            CurrentKey = _keySelector(Itr.Current);
            while (More && CurrentKey.Equals(_keySelector(Itr.Current)))
            {
                yield return Itr.Current;
                More = Itr.MoveNext();
            }
        }
    }
}