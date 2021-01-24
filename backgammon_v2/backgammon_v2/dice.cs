using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace backgammon_v2
{
    class dice
    {
        private BackgroundWorker _Worker;
        public dice()
        {
            initValues();
            initWorker();
        }
        public event EventHandler Rolled;
        public event EventHandler RollingChanged;
        public TimeSpan duration { get; set; }
        public void Roll()
        {
            if (!_Worker.IsBusy)
            {
                checkParameters();
                _Worker.RunWorkerAsync();
            }
        }
        private void checkParameters()
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new InvalidOperationException("The RollingDuration must be greater zero.");
            }
        }
        private void initWorker()
        {
            _Worker = new BackgroundWorker();
            _Worker.WorkerReportsProgress = true;
            _Worker.DoWork += OnWorkerDoWork;
            _Worker.ProgressChanged += OnWorkerProgressChanged;
            _Worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
        }
        private void initValues()
        {
           duration = TimeSpan.FromSeconds(1.5);
        }
        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var finishTime = DateTime.UtcNow + duration;
            while (finishTime > DateTime.UtcNow)
            {
                _Worker.ReportProgress(0);
                Thread.Sleep(80);
            }
        }
        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            raiseEvent(RollingChanged);
        }
        private void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            raiseEvent(Rolled);
        }
        private void raiseEvent(EventHandler handler)
        {
            var temp = handler;

            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }
    }
}
