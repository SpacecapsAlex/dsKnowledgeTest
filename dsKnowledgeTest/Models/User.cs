namespace dsKnowledgeTest.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string LastName { get; set; }
        public string Institution { get; set; }
        public string Specialization { get; set; }
        public int CourseNumber { get; set; }
        public string Group { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
