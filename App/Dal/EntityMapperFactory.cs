
using EventApp.App.Dal.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EventApp.App.Dal;

/// <summary>
/// Create entity in mapper with proxy.
/// </summary>
public class EntityMapperFactory : IRegister
{

	public void Register(TypeAdapterConfig config)
	{
		RegisterEntity<Event>(config);
	}

	private static void RegisterEntity<TEntityType>(TypeAdapterConfig config)
	{
		config.ForDestinationType<TEntityType>()
			.ConstructUsing(() => MapContext.Current.GetService<EventAppContext>().CreateProxy<TEntityType>());
	}
}
