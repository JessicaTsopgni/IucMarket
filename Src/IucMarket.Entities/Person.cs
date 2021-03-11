using Newtonsoft.Json;
using System;

namespace IucMarket.Entities
{
    public class Person
    {
        public enum RoleOptions
        {
            Admin,
            Customer,
            Provider
        }

        [JsonIgnore]
        public string Key { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleOptions Role { get; set; }
        public bool Status { get; set; }
        public Person()
        {

        }

        public Person(string key, string id, string fullName, DateTime createdAt, RoleOptions role, bool status)
        {
            Key = key;
            Id = id;
            FullName = fullName;
            CreatedAt = createdAt;
            Role = role;
            Status = status;
        }
    }
}