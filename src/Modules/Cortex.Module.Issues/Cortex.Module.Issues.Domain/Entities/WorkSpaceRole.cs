 namespace Cortex.Module.Issues.Domain.Entities
    {
        public class WorkSpaceRole
        {
            public Guid Id { get; set; }
            public  required string Name { get; set; }
            public required string Description { get; set; }

        public ICollection<WorkSpaceMember> Members { get; set; } = new List<WorkSpaceMember>();
        }
    }
