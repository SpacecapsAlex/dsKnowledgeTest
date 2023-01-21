namespace dsKnowledgeTest.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? LastName { get; set; }
        public string? Institution { get; set; }
        public string? Specialization { get; set; }
        public int? CourseNumber { get; set; }
        public string? Group { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Login { get; set; }
        public string Password { get; set; }
        public string DataCreated { get; set; }
        public string DataUpdated { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }
        public string RoleName { get; set; }
    }
}
