using Cph.DDD.Meetup.Logistics.Domain.Time;
using Cph.DDD.Meetup.Logistics.Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain.Tests
{
    public class SubscriptionsTests
    {
        public SubscriptionsTests() {

            var eventStore = new EventStore();
            var database = new Database();

            // TODO:
            // 1. Register here your event handlers using `eventBus.Register`.
            // 2. Store results in database.
            var totalTimeProjection = new TotalTimeProjection( database );

            eventStore.Register<TimePassed>( totalTimeProjection.Handle );
            //eventStore.Register<ProductItemAddedToShoppingCart>( shoppingCartDetailsProjection.Handle );
            //eventStore.Register<ProductItemRemovedFromShoppingCart>( shoppingCartDetailsProjection.Handle );
            //eventStore.Register<ShoppingCartConfirmed>( shoppingCartDetailsProjection.Handle );
            //eventStore.Register<ShoppingCartCanceled>( shoppingCartDetailsProjection.Handle );
        }
    }
}
