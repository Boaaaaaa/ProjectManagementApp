using ProjectManagement.Entities;

namespace ProjectManagement.Tests
{
    public class EntitiesUnitTest
    {
        [Fact]
        public void TestNumberOfOverdueTasks()
        {
            Project p1 = new Project() { 
                Name = "Test project #1",
                Tasks = new List<ProjectTask>()
            };

            p1.Tasks.Add(new ProjectTask() { DueDate = new DateTime(2020, 8, 5), Description = "Get milk" });
            p1.Tasks.Add(new ProjectTask() { DueDate = new DateTime(2021, 11, 24), Description = "Do laundry" });
            p1.Tasks.Add(new ProjectTask() { DueDate = new DateTime(2023, 2, 9), Description = "Learn C#" });
            p1.Tasks.Add(new ProjectTask() { DueDate = new DateTime(2023, 5, 8), Description = "Learn ASP.NET Core MVC" });
            p1.Tasks.Add(new ProjectTask() { DueDate = new DateTime(2023, 5, 31), Description = "Learn Git" });

            // Complete this test method by using a LINQ expression to query for the overdue tasks
            // and assert that it is the expected number - then ensure that the test runs and passes.

            int count = p1.Tasks.Count(d => d.DueDate > DateTime.Today);

            Assert.Equal(3, count);
        }
    }
}