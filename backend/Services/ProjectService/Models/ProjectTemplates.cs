using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

#nullable enable

public class ProjectCreationWizardTemplateCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string CategoryDescription { get; set; } = default!;
    public List<ProjectCreationWizardTemplate> ProjectTemplates { get; set; } = new();
}

public class ProjectCreationWizardTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public Guid ProjectCreationWizardTemplateCategoryId { get; set; }
    public ProjectCreationWizardTemplateCategory Category { get; set; } = default!;
    public CardConfig CardConfig { get; set; } = new();
    public Guid DetailsConfigId { get; set; }

    public DetailsConfig DetailsConfig { get; set; } = default!;
}

[Owned]
public class CardConfig
{
    public string Description { get; set; } = default!;
    public string IconName { get; set; } = default!;
    public string IconKey { get; set; } = default!;
}

public class DetailsConfig
{
    public Guid Id { get; set; } 
    public string Description { get; set; } = default!;
    public string RecommendedFor { get; set; } = default!;
    public List<WorkTypeItem> WorkTypes { get; set; } = new();
    public List<TemplateFeature> TemplateFeatures { get; set; } = new();
}

public class WorkTypeItem
{
    public Guid Id { get; set; }
    public string Value { get; set; } = default!;
    public Guid DetailsConfigId { get; set; }
    public DetailsConfig DetailsConfig { get; set; } = default!;
}

public class TemplateFeature
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string IconKey { get; set; } = default!;
    public Guid DetailsConfigId { get; set; }
    public DetailsConfig DetailsConfig { get; set; } = default!;
}

public enum ProjectCreationWizardType
{
    Team,
    Company
}

public enum ProjectAccess
{
    Open,
    Private,
    Limited
}