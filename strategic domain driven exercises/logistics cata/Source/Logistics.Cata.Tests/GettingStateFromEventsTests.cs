using Cph.DDD.Meetup.Logistics.Domain.Common;
using FluentAssertions;
using Marten;

namespace Cph.DDD.Meetup.Logistics.Domain.Tests
{
    public class GettingStateFromEventsTests : MartenTest
    {



        public GettingStateFromEventsTests()
        {

            IdProvider.Clear();

        }


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
            var containeStore = await documentSession.Get<ContainerStore>( containerStoreId, ContainerStoreDecider.Fold, cancellationToken: cancellationToken ) ?? throw new InvalidOperationException($"{nameof(ContainerStore)} was not found");

            return containeStore ?? throw new InvalidOperationException( "ContainerStore was not found!" );
        }


        [Fact]
        [Trait( "Category", "SkipCI" )]
        public void  GetNextIdFromIdProviderShoudldIncreaseId()
        {
            var id1 = IdProvider.GetNextId( typeof( ContainerStoreId ) );
            var id2 = IdProvider.GetNextId( typeof( ContainerStoreId ) );


            id1.Should().Be( Guid.Parse( "00000001-0000-0000-0000-000000000001" ) );
            id2.Should().Be( Guid.Parse( "00000001-0000-0000-0000-000000000002" ) );
        }


        [Fact]
        [Trait( "Category", "SkipCI" )]
        public async Task GettingState_ForSequenceOfEvents_ShouldSucceed()
        {
            var factoryId = new ContainerStoreId(IdProvider.GetNextId(typeof(ContainerStoreId)));

            var wareHouseAId = new ContainerStoreId( IdProvider.GetNextId( typeof( ContainerStoreId ) ));
            var wareHouseBId = new ContainerStoreId( IdProvider.GetNextId( typeof( ContainerStoreId ) ));

            var now = new DateTime( 2024, 1, 1, 8, 0, 0, 0 );

            var clock = new Clock( now );

            var events = new object[]
            {

                new ContainerStoreInitialized( factoryId, new FreightLocationId(1),"Factory", new HashSet<ContainerState>()),
                new ContainerUnloadedAt(factoryId, new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), wareHouseAId, clock.Time),
                new ContainerUnloadedAt(factoryId, new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), wareHouseAId, clock.Time),
                new ContainerUnloadedAt(factoryId, new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), wareHouseBId, clock.Time),
                new ContainerUnloadedAt(factoryId, new ContainerId(IdProvider.GetNextId( typeof( ContainerId ))), wareHouseBId, clock.Time),
            };

            await AppendEvents( factoryId.Id, events, CancellationToken.None );

                var factory = await GetContainerStore( DocumentSession, factoryId.Id, CancellationToken.None );

            factory.Id.Should().Be( factoryId );
            factory.Containers.Should().HaveCount( 4 );

            factory.Containers.ElementAt(0).Destination.Should().Be(wareHouseAId );
            factory.Containers.ElementAt( 1 ).Destination.Should().Be(wareHouseAId );
            factory.Containers.ElementAt( 2 ).Destination.Should().Be(wareHouseBId );
            factory.Containers.ElementAt( 3 ).Destination.Should().Be( wareHouseBId );
        }
    }
}

