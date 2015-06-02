using System.Messaging;

namespace OnlineStore.Events.Bus
{
    public class MsmqBusOptions
    {
        public bool SharedModeDenyReceive { get; set; }
        public bool EnableCache { get; set; }
        public QueueAccessMode QueueAccessMode { get; set; }
        public string Path { get; set; }
        public bool UseInternalTransaction { get; set; }
        public IMessageFormatter MessageFormatter { get; set; }

        public MsmqBusOptions(string path, bool sharedModeDenyReceive, bool enableCache, QueueAccessMode queueAccessMode, bool useInternalTransaction, IMessageFormatter messageFormatter)
        {
            this.SharedModeDenyReceive = sharedModeDenyReceive;
            this.EnableCache = enableCache;
            this.QueueAccessMode = queueAccessMode;
            this.Path = path;
            this.UseInternalTransaction = useInternalTransaction;
            this.MessageFormatter = messageFormatter;
        }

        public MsmqBusOptions(string path)
            : this(path, false, false, QueueAccessMode.SendAndReceive, false, new XmlMessageFormatter())
        { }

        public MsmqBusOptions(string path, bool useInternalTransaction)
            : this(path, false, false, QueueAccessMode.SendAndReceive, useInternalTransaction, new XmlMessageFormatter())
        { }
    }
}