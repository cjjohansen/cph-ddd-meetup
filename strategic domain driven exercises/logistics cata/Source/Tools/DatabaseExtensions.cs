using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Domain.Tools;

public static class DatabaseExtensions
{
    public static void GetAndStore<T>( this Database database, Guid id, ulong version, Func<T, T> update )
        where T : class, IVersioned, new()
    {
        var item = database.Get<T>( id );

        if ( item == null )
            throw new Exception( $"Item with id: '{id}' and expected version: {version} not found!" );

        if ( item.Version >= version ) return;

        database.Store( id, version, update( item! ) );
    }
}
