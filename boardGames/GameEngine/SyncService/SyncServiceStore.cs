using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameEngine.SyncService
{
    public sealed class SyncEntry<TValue>
    {
        [JsonProperty("value")]
        public readonly TValue Value;

        [JsonProperty("datetime")]
        public readonly DateTime DateTime;

        [JsonProperty("synctoken")]
        public readonly SyncToken SyncToken;

        [JsonConstructor()]
        public SyncEntry(TValue value, DateTime dateTime, SyncToken syncToken)
        {
            Value = value;
            DateTime = dateTime;
            SyncToken = syncToken;
        }
    }
    public interface ISyncServiceStore<TValue>
    {
        SyncToken AddEntry(TValue value);
        IReadOnlyCollection<SyncEntry<TValue>> GetEntries(SyncToken syncToken, int maxCount = 10);
        SyncToken Head();
    }

    public class SyncServiceStore<TValue> : ISyncServiceStore<TValue>
    {
        private readonly int _capacity;
        private readonly LinkedList<SyncEntry<TValue>> _store;
        private System.Threading.ReaderWriterLockSlim _readerWriterLock;
        private SyncToken _syncTokenHead;


        public SyncServiceStore(int capacity = Int32.MaxValue)
        {
            Debug.Assert(capacity > 1, "SyncServiceStore capacity should be positive number.");

            _capacity = capacity;
            _syncTokenHead = new SyncToken();
            _store = new LinkedList<SyncEntry<TValue>>();
            _readerWriterLock = new ReaderWriterLockSlim();
        }

        public SyncToken Head()
        {
            return _syncTokenHead;
        }

        public SyncToken AddEntry(TValue value)
        {
            _readerWriterLock.EnterWriteLock();
            try
            {
                SyncEntry<TValue> syncEntry = new SyncEntry<TValue>(
                       value: value,
                       dateTime: DateTime.UtcNow,
                       syncToken: new SyncToken(Guid.NewGuid()));
                _store.AddLast(syncEntry);

                if (_store.Count > _capacity)
                {
                    _store.RemoveFirst();
                }

                _syncTokenHead = syncEntry.SyncToken;
                return _syncTokenHead;
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }

        public IReadOnlyCollection<SyncEntry<TValue>> GetEntries(SyncToken syncToken, int maxCount = 10)
        {
            if (syncToken.CompareTo(_syncTokenHead) == 0)
            {
                return new List<SyncEntry<TValue>>();
            }

            _readerWriterLock.EnterReadLock();
            try
            {
                if (syncToken == SyncToken.Empty)
                {
                    return _store.Take(maxCount).ToList();
                }

                var subStore = _store.SkipWhile(e => e.SyncToken.CompareTo(syncToken) != 0);

                // If token not found throw exception
                if (subStore.ElementAtOrDefault(0) == null)
                {
                   // throw new SyncTokenNotFound();
                }

                return subStore.Skip(1).Take(maxCount).ToList();
            }
            finally
            {
                _readerWriterLock.ExitReadLock();
            }
        }
    }
}
