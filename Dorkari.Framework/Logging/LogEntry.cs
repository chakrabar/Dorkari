using System;

namespace Dorkari.Framework.Logging
{
    class LogEntry
    {
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string User { get; set; }
        public string HostName { get; set; }
    }
}
