namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ITimeProvider
{
	DateTime Now { get; }
}