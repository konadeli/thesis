using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aade.ViewModel
{
    public class MessagesForMe
    {
        public string Id { get; set; }
        public string PolitisUserId { get; set; }
        public string PolitisName { get; set; }
        public string PolitisEmail { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public short Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
