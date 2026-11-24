using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waypoint.Models;

/// <summary>A place the team has visited or wants to visit.</summary>
public class Destination
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>URL-friendly identifier used in /destinations/{slug}. Unique.</summary>
    [Required, StringLength(100), RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    public string Slug { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Continent { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string Summary { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Best time to visit"), StringLength(100)]
    public string BestTimeToVisit { get; set; } = string.Empty;

    [Display(Name = "Avg. daily budget (USD)"), Column(TypeName = "decimal(8,2)"), Range(0, 10000)]
    [DataType(DataType.Currency)]
    public decimal AverageDailyBudgetUsd { get; set; }

    /// <summary>Team rating from 1 (skip it) to 5 (go now).</summary>
    [Range(1, 5)]
    public int Rating { get; set; }

    [Display(Name = "Recommended by"), StringLength(50)]
    public string RecommendedBy { get; set; } = string.Empty;

    /// <summary>True if a team member has been there; false if it's on the wish list.</summary>
    public bool Visited { get; set; }

    /// <summary>App-relative path to the photo, e.g. /images/destinations/kyoto.webp.</summary>
    [Display(Name = "Photo"), StringLength(200)]
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>Describes what the photo shows, for screen readers.</summary>
    [Display(Name = "Photo description"), StringLength(300)]
    public string ImageAlt { get; set; } = string.Empty;

    /// <summary>Photographer name shown under the photo.</summary>
    [Display(Name = "Photo credit"), StringLength(100)]
    public string ImageCredit { get; set; } = string.Empty;

    /// <summary>Link to the photo's source page and license.</summary>
    [Display(Name = "Photo source"), StringLength(300), Url]
    public string ImageCreditUrl { get; set; } = string.Empty;
}
