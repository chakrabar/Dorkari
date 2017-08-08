using Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models.Enums;
using System;

namespace Dorkari.Framework.Web.Areas.SecureStatefulSPAFramework.Models
{
    public class BookTicketSession
    {
        public BookTicketStep NextStep { get; set; }
        public string MovieName { get; set; }
        public DateTime MovieTime { get; set; }
        public int TicketCount { get; set; }
        public decimal TicketCost { get; set; }
        public bool IsPaid { get; set; }
    }
}