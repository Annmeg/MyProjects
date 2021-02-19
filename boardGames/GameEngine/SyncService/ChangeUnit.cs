using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.SyncService
{
    public abstract class Change
    {
        public enum ChangeTypes
        {
            GameStart,
            Move,
            AddBall,
            RemoveBlock,
            Score,
            Level,
            GameOver,
        };
        public readonly ChangeTypes ChangeType;
        public Change(ChangeTypes changeType)
        {
            ChangeType = changeType;
        }
    }

    public sealed class ChangeGameStart : Change
    {
        public ChangeGameStart() : base(ChangeTypes.GameStart)
        {
        }
    }

    class ChangeUnit
    {
    }
}
