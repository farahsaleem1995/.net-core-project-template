using AutoMapper;
using System.Reflection;

namespace DotnetCoreTemplate.Application.Shared.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		var assembly = Assembly.GetExecutingAssembly();

		ApplyMappingsFromAssembly(assembly);
	}

	private void ApplyMappingsFromAssembly(Assembly assembly)
	{
		var mappingTypes = assembly.GetExportedTypes().Where(IsMappingType).ToList();

		foreach (var mappingType in mappingTypes)
		{
			var mapInstance = CreateMappingInstance(mappingType);

			mapInstance.Mapping(this);
		}
	}

	private static bool IsMappingType(Type type)
	{
		var ignoredTypes = new[] { typeof(IHasMap), typeof(IMapFrom<>), typeof(IMapTo<>) };

		return typeof(IHasMap).IsAssignableFrom(type) && !ignoredTypes.Contains(type);
	}

	private static IHasMap CreateMappingInstance(Type mappingType)
	{
		var instance = Activator.CreateInstance(mappingType);

		if (instance == null)
		{
			throw new InvalidOperationException($"Could not create mapping instance of type '{mappingType}'.");
		}

		return (IHasMap)instance;
	}
}