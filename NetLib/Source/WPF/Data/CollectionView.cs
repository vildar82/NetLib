namespace NetLib.WPF.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using JetBrains.Annotations;

    public interface ICollectionView<out T> : IEnumerable<T>, ICollectionView
    {
        IEnumerable<T> SourceCollectionGeneric { get; }
    }

    [PublicAPI]
    public class CollectionView<T> : ICollectionView<T>
    {
        private readonly ICollectionView _collectionView;

        public CollectionView(List<T> items)
        {
            _collectionView = new ListCollectionView(items);
        }

        private class ColEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator _enumerator;
            public ColEnumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
               return _enumerator.MoveNext();
            }

            public void Reset()
            {
               _enumerator.Reset();
            }

            public T Current => (T) _enumerator.Current;

            object IEnumerator.Current => Current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ColEnumerator(_collectionView.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collectionView.GetEnumerator();
        }

        public bool Contains(object item)
        {
            return _collectionView.Contains(item);
        }

        public void Refresh()
        {
            _collectionView.Refresh();
        }

        public IDisposable DeferRefresh()
        {
            return _collectionView.DeferRefresh();
        }

        public bool MoveCurrentToFirst()
        {
            return _collectionView.MoveCurrentToFirst();
        }

        public bool MoveCurrentToLast()
        {
            return _collectionView.MoveCurrentToLast();
        }

        public bool MoveCurrentToNext()
        {
            return _collectionView.MoveCurrentToNext();
        }

        public bool MoveCurrentToPrevious()
        {
            return _collectionView.MoveCurrentToPrevious();
        }

        public bool MoveCurrentTo(object item)
        {
            return _collectionView.MoveCurrentTo(item);
        }

        public bool MoveCurrentToPosition(int position)
        {
            return _collectionView.MoveCurrentToPosition(position);
        }

        public CultureInfo Culture
        {
            get => _collectionView.Culture;
            set => _collectionView.Culture = value;
        }

        public IEnumerable SourceCollection => _collectionView.SourceCollection;

        public Predicate<object> Filter
        {
            get => _collectionView.Filter;
            set => _collectionView.Filter = value;
        }
        public bool CanFilter => _collectionView.CanFilter;

        public SortDescriptionCollection SortDescriptions => _collectionView.SortDescriptions;

        public bool CanSort => _collectionView.CanSort;

        public bool CanGroup => _collectionView.CanGroup;

        public ObservableCollection<GroupDescription> GroupDescriptions => _collectionView.GroupDescriptions;

        public ReadOnlyObservableCollection<object> Groups => _collectionView.Groups;

        public bool IsEmpty => _collectionView.IsEmpty;

        public object CurrentItem => _collectionView.CurrentItem;

        public int CurrentPosition => _collectionView.CurrentPosition;

        public bool IsCurrentAfterLast => _collectionView.IsCurrentAfterLast;

        public bool IsCurrentBeforeFirst => _collectionView.IsCurrentBeforeFirst;

        public event CurrentChangingEventHandler CurrentChanging
        {
            add
            {
                lock (objectLock)
                {
                    _collectionView.CurrentChanging += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    _collectionView.CurrentChanging -= value;
                }
            }
        }

        private readonly object objectLock = new object();
        public event EventHandler CurrentChanged
        {
            add {
                lock (objectLock)
                {
                    _collectionView.CurrentChanged += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    _collectionView.CurrentChanged -= value;
                }
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (objectLock)
                {
                    _collectionView.CollectionChanged += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    _collectionView.CollectionChanged -= value;
                }
            }
        }

        public IEnumerable<T> SourceCollectionGeneric => _collectionView.Cast<T>();
    }
}
