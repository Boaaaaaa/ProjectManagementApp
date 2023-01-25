using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Entities;
using ProjectManagementApp.DataAccess;
using ProjectManagementApp.Models;
using System.IO;

namespace ProjectManagementApp.Controllers
{
    public class ProjectController : Controller
    {
        public ProjectController(ProjectManagementDbContext projectManagementDbContext)
        {
            _projectManagementDbContext = projectManagementDbContext;
        }

        [HttpGet("/projects")]
        public IActionResult GetAllProjects()
        {
            var projects = _projectManagementDbContext.Projects
                    .Include(p => p.Employees)
                    .Include(p => p.Tasks)
                    .OrderByDescending(p => p.DateCreated)
                    .ToList();

            return View("Items", projects);
        }

        [HttpGet("/projects/{id}")]
        public IActionResult GetProjectById(int id)
        {
            var project = _projectManagementDbContext.Projects
                    .Include(p => p.Employees)
                    .Include(p => p.Tasks)
                    .Where(p => p.ProjectId == id)
                    .FirstOrDefault();

            if (project == null)
                return NotFound();

            ProjectDetailsViewModel projectDetailsViewModel = new ProjectDetailsViewModel()
            {
                ActiveProject = project
            };

            return View("Details", projectDetailsViewModel);
        }

        [HttpGet("/projects/add-request")]
        [Authorize()]
        public IActionResult GetAddProjectRequest()
        {
            return View("AddProject", new Project());
        }

        [HttpPost("/projects")]
        [Authorize()]
        public IActionResult AddNewProject(Project project)
        {
            if (ModelState.IsValid)
            {
                _projectManagementDbContext.Projects.Add(project);
                _projectManagementDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"The project \"{project.Name}\" was added.";

                return RedirectToAction("GetAllProjects", "Project");
            }
            else
            {
                return View("AddProject", project);
            }
        }

        [HttpGet("/projects/{id}/edit-request")]
        [Authorize()]
        public IActionResult GetEditRequestById(int id)
        {
            var project = _projectManagementDbContext.Projects.Find(id);
            return View("EditProject", project);
        }

        [HttpPost("/projects/edit-requests")]
        [Authorize()]
        public IActionResult ProcessEditRequest(Project project)
        {
            if (ModelState.IsValid)
            {
                _projectManagementDbContext.Projects.Update(project);
                _projectManagementDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"The project \"{project.Name}\" was updated.";

                return RedirectToAction("GetAllProjects", "Project");
            }
            else
            {
                return View("EditProject", project);
            }
        }

        [HttpGet("/projects/{id}/delete-request")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDeleteRequestById(int id)
        {
            var project = _projectManagementDbContext.Projects.Find(id);
            return View("DeleteConfirmation", project);
        }

        [HttpPost("/projects/{id}/delete-requests")]
        [Authorize(Roles = "Admin")]
        public IActionResult ProcessDeleteRequestById(int id)
        {
            var project = _projectManagementDbContext.Projects.Find(id);

            _projectManagementDbContext.Projects.Remove(project);
            _projectManagementDbContext.SaveChanges();

            TempData["LastActionMessage"] = $"The project \"{project.Name}\" was deleted.";

            return RedirectToAction("GetAllProjects", "Project");
        }

        // Those are what I added for final exam

        [HttpPost("/projects/{id}/employee")]
        public IActionResult AddEmployeeToProjectById(int id, ProjectDetailsViewModel projectDetailsViewModel)
        {
            var project = _projectManagementDbContext.Projects
                .Include(m => m.Employees).Where(m => m.ProjectId == id).FirstOrDefault();

            project.Employees.Add(projectDetailsViewModel.NewEmployee);

            _projectManagementDbContext.Projects.Update(project);
            _projectManagementDbContext.SaveChanges();

            return RedirectToAction("GetProjectById", "Project", new { id = id });
        }

        [HttpPost("/projects/{id}/tasks")]
        public IActionResult AddTaskToProjectById(int id, ProjectDetailsViewModel projectDetailsViewModel)
        {
            var project = _projectManagementDbContext.Projects
                .Include(m => m.Tasks).Where(m => m.ProjectId == id).FirstOrDefault();

            project.Tasks.Add(projectDetailsViewModel.NewProjectTask);

            _projectManagementDbContext.Projects.Update(project);
            _projectManagementDbContext.SaveChanges();

            return RedirectToAction("GetProjectById", "Project", new { id = id });
        }

        private ProjectManagementDbContext _projectManagementDbContext;
    }
}
