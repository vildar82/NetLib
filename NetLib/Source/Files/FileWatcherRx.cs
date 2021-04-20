namespace NetLib
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Linq;

    public class FileWatcherRx
    {
        public IObservable<EventPattern<FileSystemEventArgs>> Changed { get; } = Observable.Empty<EventPattern<FileSystemEventArgs>>();

        public IObservable<EventPattern<FileSystemEventArgs>> Created { get; } = Observable.Empty<EventPattern<FileSystemEventArgs>>();

        public IObservable<EventPattern<FileSystemEventArgs>> Deleted { get; } = Observable.Empty<EventPattern<FileSystemEventArgs>>();

        public IObservable<EventPattern<ErrorEventArgs>> Error { get; set; }

        public IObservable<EventPattern<RenamedEventArgs>> Renamed { get; set; } = Observable.Empty<EventPattern<RenamedEventArgs>>();

        public FileSystemWatcher Watcher { get; }

        public FileWatcherRx(
            string path,
            string filter,
            NotifyFilters notifyFilters = NotifyFilters.LastWrite,
            WatcherChangeTypes changeTypes = WatcherChangeTypes.Changed)
        {
            Watcher = new FileSystemWatcher
            {
                Path = path,
                IncludeSubdirectories = false,
                NotifyFilter = notifyFilters,
                Filter = filter,
                EnableRaisingEvents = true,
            };

            if (changeTypes.HasFlag(WatcherChangeTypes.Changed))
                Changed = Observable.FromEventPattern<FileSystemEventArgs>(Watcher, "Changed");

            if (changeTypes.HasFlag(WatcherChangeTypes.Created))
                Created = Observable.FromEventPattern<FileSystemEventArgs>(Watcher, "Created");

            if (changeTypes.HasFlag(WatcherChangeTypes.Deleted))
                Deleted = Observable.FromEventPattern<FileSystemEventArgs>(Watcher, "Deleted");

            if (changeTypes.HasFlag(WatcherChangeTypes.Renamed))
                Renamed = Observable.FromEventPattern<RenamedEventArgs>(Watcher, "Renamed");

            Error = Observable.FromEventPattern<ErrorEventArgs>(Watcher, nameof(Watcher.Error));
        }
    }
}