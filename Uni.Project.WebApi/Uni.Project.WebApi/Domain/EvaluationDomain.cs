namespace Uni.Project.WebApi.Domain
{
    public class EvaluationDomain
    {
        public EvaluationDomain(Guid userId, DateTime creationTime, DateTime updateTime, int score, string description)
        {
            UserId = userId;
            CreationTime = creationTime;
            UpdateTime = updateTime;
            Score = score;
            Description = description;
        }

        public EvaluationDomain() { }

        public Guid EvaluationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }

        public virtual UserDomain UserIdNavigation { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is EvaluationDomain domain;
        }
    }
}
