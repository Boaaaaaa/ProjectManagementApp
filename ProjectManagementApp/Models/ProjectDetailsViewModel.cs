using ProjectManagement.Entities;

namespace ProjectManagementApp.Models
{
    public class ProjectDetailsViewModel
    {
        public Project ActiveProject { get; set; }
        public Employee NewEmployee { get; set; }
        public ProjectTask NewProjectTask { get; set; }
    }
}
