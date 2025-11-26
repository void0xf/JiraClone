using Microsoft.EntityFrameworkCore;

public static class ProjectTemplatesSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(context.Categories))
        {
            return;
        }

        var categories = new List<ProjectCreationWizardTemplateCategory>
        {
            new ProjectCreationWizardTemplateCategory
            {
                Id = Guid.NewGuid(),
                Name = "Software development",
                CategoryDescription = "Plan, track and release great software. Get up and running quickly with templates that suit the way your team works. Plus, integrations for DevOps teams that want to connect work across their entire toolchain.",
                ProjectTemplates = new List<ProjectCreationWizardTemplate>
                {
                    CreateTemplate(
                        "Kanban",
                        "Jira",
                        "Visualize and advance your project forward using issues on a powerful board.",
                        "board-kanban",
                        "Kanban (the Japanese word for \"visual signal\") is all about helping teams visualize their work, limit work-in-progress, and maximize efficiency. Use the Kanban template to increase planning flexibility, reduce bottlenecks, and promote transparency throughout the development cycle.",
                        "Recommended for teams who want to visualize work and limit work-in-progress.",
                        new List<string> { "Epic", "Story", "Bug", "Task", "Sub-task" },
                        new List<TemplateFeature>
                        {
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Track work using a simple board", Description = "Work items are represented visually on your kanban board, allowing teams to track the status of work at any time. The columns on your board represent each step in your team's workflow.", IconKey = "assets/images/wizard/kanban-feature-board.svg" },
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Use the board to limit work in progress", Description = "Set the maximum amount of work that can exist in each status with work in progress (WIP) limits. By limiting work in progress, you can improve team focus and identify inefficiencies.", IconKey = "assets/images/wizard/kanban-feature-wip.svg" },
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Continuously improve with agile reports", Description = "One of the key tenets of kanban is optimizing flow for continuous delivery. Agile reports, like the cumulative flow diagram, help ensure your team is consistently delivering maximum value.", IconKey = "assets/images/wizard/kanban-feature-reports.svg" }
                        }
                    ),
                    CreateTemplate(
                        "Scrum",
                        "Jira",
                        "Sprint toward your project goals with a board, backlog, and timeline.",
                        "board-scrum",
                        "Scrum is an agile framework that helps teams work together. Scrum encourages teams to learn through experiences, self-organize while working on a problem, and reflect on their wins and losses to continuously improve.",
                        "Recommended for teams who work in sprints.",
                        new List<string> { "Epic", "Story", "Bug", "Task", "Sub-task" },
                        new List<TemplateFeature>
                        {
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Plan and prioritize with a backlog", Description = "The backlog is your single source of truth for all the work your team needs to do. Prioritize items here before moving them into a sprint.", IconKey = "assets/images/wizard/scrum-feature-backlog.svg" },
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Stay on track with Sprints", Description = "Create time-boxed iterations called sprints to complete a set amount of work. This helps focus the team and provides a predictable delivery cadence.", IconKey = "assets/images/wizard/scrum-feature-sprints.svg" }
                        }
                    ),
                    CreateTemplate(
                        "Bug tracking",
                        "Jira",
                        "Manage a list of development tasks and bugs.",
                        "bug",
                        "Great for teams who don't need a board but want to track bugs and tasks in a list view. Keeps things simple and focused on resolution.",
                        "Recommended for teams focused on bug tracking and resolution.",
                        new List<string> { "Bug", "Task", "Sub-task" },
                        new List<TemplateFeature>
                        {
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Capture bugs efficiently", Description = "Standardized bug reporting fields ensure your team gets all the info they need to fix issues fast.", IconKey = "assets/images/wizard/bug-feature-capture.svg" }
                        }
                    )
                }
            },
            new ProjectCreationWizardTemplateCategory
            {
                Id = Guid.NewGuid(),
                Name = "Service management",
                CategoryDescription = "Plan, track and release great software. Get up and running quickly with templates that suit the way your team works. Plus, integrations for DevOps teams that want to connect work across their entire toolchain.",
                ProjectTemplates = new List<ProjectCreationWizardTemplate>
                {
                    CreateTemplate(
                        "IT Service Management",
                        "Jira Service Management",
                        "Manage IT requests and incidents with a service desk.",
                        "server",
                        "Everything IT teams need to manage service requests, incidents, problems, and changes.",
                        "Recommended for IT teams managing service requests.",
                        new List<string> { "Service Request", "Incident", "Problem", "Change" },
                        new List<TemplateFeature>
                        {
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Self-service portal", Description = "Empower employees to find answers and request help through a simple online portal.", IconKey = "assets/images/wizard/service-feature-portal.svg" }
                        }
                    )
                }
            },
            new ProjectCreationWizardTemplateCategory
            {
                Id = Guid.NewGuid(),
                Name = "Work management",
                CategoryDescription = "Plan, track and release great software. Get up and running quickly with templates that suit the way your team works. Plus, integrations for DevOps teams that want to connect work across their entire toolchain.",
                ProjectTemplates = new List<ProjectCreationWizardTemplate>
                {
                    CreateTemplate(
                        "Project Management",
                        "Jira Work Management",
                        "Manage any business project with lists, calendars, and timelines.",
                        "clipboard-list",
                        "Perfect for HR, Marketing, Legal, and other business teams to track tasks and hit deadlines.",
                        "Recommended for business teams tracking tasks and deadlines.",
                        new List<string> { "Task", "Sub-task" },
                        new List<TemplateFeature>
                        {
                            new TemplateFeature { Id = Guid.NewGuid(), Title = "Calendar view", Description = "See due dates and project milestones on a visual calendar.", IconKey = "assets/images/wizard/work-feature-calendar.svg" }
                        }
                    )
                }
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static ProjectCreationWizardTemplate CreateTemplate(
        string name,
        string productName,
        string cardDescription,
        string iconName,
        string detailsDescription,
        string recommendedFor,
        List<string> workTypes,
        List<TemplateFeature> features)
    {
        var detailsConfigId = Guid.NewGuid();
        var detailsConfig = new DetailsConfig
        {
            Id = detailsConfigId,
            Description = detailsDescription,
            RecommendedFor = recommendedFor,
            WorkTypes = workTypes.Select(wt => new WorkTypeItem { Id = Guid.NewGuid(), Value = wt, DetailsConfigId = detailsConfigId }).ToList(),
            TemplateFeatures = features.Select(f => { f.DetailsConfigId = detailsConfigId; return f; }).ToList()
        };

        return new ProjectCreationWizardTemplate
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProductName = productName,
            CardConfig = new CardConfig
            {
                Description = cardDescription,
                IconName = iconName,
                IconKey = iconName // Assuming IconKey is same as IconName or derived
            },
            DetailsConfigId = detailsConfigId,
            DetailsConfig = detailsConfig
        };
    }
}
