using Cph.DDD.Meetup.Logistics.Domain.Common;
using FluentAssertions;
using Marten;

namespace Cph.DDD.Meetup.Logistics.Domain.Tests
{
    public class GettingStateFromEventsTests : MartenTest
    {
        /// <summary>
        /// Solution - Immutable entity
        /// </summary>
        /// <param name="documentSession"></param>
        /// <param name="containerStoreId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<ContainerStore> GetContainerStore( IDocumentSession documentSession, Guid containerStoreId,
            CancellationToken cancellationToken )
        {
            var containeStore = await documentSession.Events.AggregateStreamAsync<ContainerStore>( containerStoreId, token: cancellationToken );

            return containeStore ?? throw new InvalidOperationException( "ContainerStore was not found!" );

        }

        [Fact]
        [Trait( "Category", "SkipCI" )]
        public async Task GettingState_ForSequenceOfEvents_ShouldSucceed()
        {
            var factoryId = IdProvider.GetNextId(typeof(ContainerStoreId));
            IdProvider.GetNextId( typeof( ContainerId ) );


            var now = new DateTime( 2024, 1, 1, 8, 0, 0, 0 );

            var clock = new Clock( now );


            var events = new object[]
            {

                new ContainerStoreInitialized( new ContainerStoreId(factoryId), new FreightLocationId(1),"Factory", new HashSet<ContainerState>()),
                new ContainerPlacedAt(new ContainerStoreId(factoryId), new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), new DestinationCode("A"), clock.Time),
                new ContainerPlacedAt(new ContainerStoreId(factoryId), new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), new DestinationCode("A"), clock.Time),
                new ContainerPlacedAt(new ContainerStoreId(factoryId), new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), new DestinationCode("B"), clock.Time),
                new ContainerPlacedAt(new ContainerStoreId(factoryId), new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), new DestinationCode("B"), clock.Time),
            };

            await AppendEvents( factoryId, events, CancellationToken.None );

            var factory = await GetContainerStore( DocumentSession, factoryId, CancellationToken.None );

            factory.Id.Id.Should().Be( factoryId );
            factory.Containers.Should().HaveCount( 4 );

            factory.Containers.ElementAt(0).Destination.Id.Should().Be("A" );
            factory.Containers.ElementAt( 1 ).Destination.Id.Should().Be( "A" );
            factory.Containers.ElementAt( 2 ).Destination.Id.Should().Be( "B" );
            factory.Containers.ElementAt( 3 ).Destination.Id.Should().Be( "B" );
        }
    }
}

