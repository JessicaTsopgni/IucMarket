using System;

namespace IucMarket.Entities
{
    public class Person
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public Person()
        {

        }

        public Person(string id, string fullName, DateTime createdAt, bool status)
        {
            Id = id;
            FullName = fullName;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}