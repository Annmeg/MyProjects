using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.SyncService
{
    public sealed class SyncToken : IComparable
    {
        private readonly string _token;

        public static readonly SyncToken Empty = new SyncToken();

        public SyncToken()
        {
            _token = string.Empty;
        }

        public SyncToken(Guid guid)
        {
          //_token = new ShortGuid(guid).ToString();
        }

        public int CompareTo(object obj)
        {
            SyncToken other = obj as SyncToken;
            if (other == null)
                return -1;
            return _token.CompareTo(other._token);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }

        public override string ToString()
        {
            return _token;
        }
    }

}

