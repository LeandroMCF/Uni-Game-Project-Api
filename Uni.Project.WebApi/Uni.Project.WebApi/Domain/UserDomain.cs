using Uni.Project.WebApi.Enum;

namespace Uni.Project.WebApi.Domain
{
    public class UserDomain
    {
        public UserDomain() { }

        public UserDomain(string? name, string? email, string? password, DateTime creationTime, DateTime updateTime, StatusEnum status, UserEnum permission)
        {
            Name = name;
            Email = email;
            Password = password;
            CreationTime = creationTime;
            UpdateTime = updateTime;
            Status = status;
            Permission = permission;
        }

        public Guid UserId { get; set; }
        public string ?Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Download { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public StatusEnum Status { get; set; }
        public UserEnum Permission { get; set; }

        public virtual ICollection<EvaluationDomain> Evaluations { get; set; }
    }
}
