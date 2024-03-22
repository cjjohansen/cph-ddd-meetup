using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain.Common
{
    public static class DocumentSessionExtensions
    {
        public static async Task<T> Get<T>(this IDocumentSession documentSession, Guid id, Func<IEnumerable<IEvent>,T> fold, CancellationToken cancellationToken )
        {
            var eventStream   = await documentSession.Events.FetchStreamAsync(id, token: cancellationToken);

            return fold( eventStream.Select( x => x.Data ).OfType<IEvent>() );
        }
    }}
