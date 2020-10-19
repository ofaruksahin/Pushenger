using Dapper.Contrib.Extensions;
using Pushenger.Core.Enums;
using System;

namespace Pushenger.Core.Entities
{
    public class BaseEntity
    {
        [ExplicitKey]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int CreatorId { get; set; }

        public enumRecordStatus Status { get; set; }

        public BaseEntity()
        {
            CreationDate = DateTime.Now;
            Status = enumRecordStatus.Active;
        }
    }
}
