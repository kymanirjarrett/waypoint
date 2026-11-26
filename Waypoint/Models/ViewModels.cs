namespace Waypoint.Models;

// Typed view models replace ViewData string keys, so a renamed or mistyped field fails at compile time, not at runtime.

/// <summary>Home page: the top-rated destination as the hero photo, then the next few as picks.</summary>
public record HomeIndexViewModel(Destination? Hero, IReadOnlyList<Destination> TopPicks, int DestinationCount);

/// <summary>Destination card grid with its continent filter.</summary>
public record DestinationIndexViewModel(IReadOnlyList<Destination> Destinations, IReadOnlyList<string> Continents, string? SelectedContinent);

/// <summary>Every destination as a table, in the order named by <paramref name="Sort"/>.</summary>
public record DestinationTableViewModel(IReadOnlyList<Destination> Destinations, string Sort);

/// <summary>One destination plus the next one alphabetically for the "Next destination" link.</summary>
public record DestinationDetailsViewModel(Destination Destination, Destination? Next);
