﻿
namespace Domain.Common
{
    //Clase base
    public abstract class AuditableBaseEntity 
    {
        public virtual int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
    }
}
