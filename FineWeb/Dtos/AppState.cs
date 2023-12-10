namespace FineWeb.Dtos;

public record AppState(bool Healthy, bool EnableDelay, string AppName, int Count);