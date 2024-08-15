using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteARP
{
    internal sealed class ApplicationService
    {
        private ApplicationService() { }
        private static readonly ApplicationService _instance = new ApplicationService();
        internal static ApplicationService Instance { get { return _instance; } }

        private Prism.Events.IEventAggregator _eventAggregator;
        internal Prism.Events.IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                    _eventAggregator = new Prism.Events.EventAggregator();
                return _eventAggregator;
            }
        }
    }

    /// <summary>
    /// send bool event from class ApplicationService
    /// </summary>
    public class UpdatedEvent : PubSubEvent<bool>
    {
    }
    /// <summary>
    /// Program Shutdown by click X
    /// </summary>
    public class UpdatedEventShutdown : PubSubEvent<bool>
    {
    }
    public class UpdatedSqlTableEvent : PubSubEvent<int>
    {
    }
    public class UpdatedWLEvent : PubSubEvent<bool>
    {
    }

}
