using System;
using System.ComponentModel.DataAnnotations;
using Aade.Services;

namespace Aade.Models.Messages
{
    public class Messages : IEntity
    {
        [Key]
        public string Id { get; set; }
        public string PolitisUserId { get; set; }
        public string AadeuserId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Message { get; set; }
        public short Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
