using System;

namespace Entity
{
    public interface IEntity
    {
       
    }
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string CreateUser { get; set; }

        public string LastUpdateUser { get; set; }
    }
}
