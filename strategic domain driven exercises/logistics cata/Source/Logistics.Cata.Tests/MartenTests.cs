using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marten;
using static System.Formats.Asn1.AsnWriter;

namespace Cph.DDD.Meetup.Logistics.Domain.Tests
{
    public abstract class MartenTest : IDisposable
    {
        private readonly DocumentStore documentStore;
        protected readonly IDocumentSession DocumentSession;

        protected MartenTest()
        {
            var options = new StoreOptions();
            options.Connection(
                "PORT = 5432; HOST = localhost; TIMEOUT = 15; POOLING = True; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'" );
            options.UseDefaultSerialization( nonPublicMembersStorage: NonPublicMembersStorage.All );
            options.DatabaseSchemaName = options.Events.DatabaseSchemaName = "IntroductionToEventSourcing";
            options.Events.AddEventType<ContainerUnloadedAt>();

            documentStore = new DocumentStore( options );
            DocumentSession = documentStore.LightweightSession();

            documentStore.Advanced.Clean.CompletelyRemoveAll();

        }

        protected Task AppendEvents( Guid streamId, object[] events, CancellationToken ct )
        {
            DocumentSession.Events.Append(
                streamId,
                events
            );
            return DocumentSession.SaveChangesAsync( ct );
        }

        public virtual void Dispose()
        {
            DocumentSession.Dispose();
            documentStore.Dispose();
        }
    }
}
