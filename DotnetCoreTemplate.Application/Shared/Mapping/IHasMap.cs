using AutoMapper;

namespace DotnetCoreTemplate.Application.Shared.Mapping;

public interface IHasMap
{
	void Mapping(Profile profile);
}

public interface IMapFrom<TSource> : IHasMap
{
	void IHasMap.Mapping(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
}

public interface IMapTo<TDestination> : IHasMap
{
	void IHasMap.Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(TDestination));
}