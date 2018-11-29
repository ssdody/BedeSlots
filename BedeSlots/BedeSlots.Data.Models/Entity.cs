using BedeSlots.Data.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public abstract class Entity : IAuditable, IDeletable
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
