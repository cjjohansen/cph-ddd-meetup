using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weasel.Core.Migrations;
using Cph.DDD.Meetup.Logistics.Domain.Tools;


namespace Cph.DDD.Meetup.Logistics.Domain.Time
{


    public class TotalTimeProjection
    {
        private readonly Database database;
        public TotalTimeProjection( Database database ) => this.database = database;

        public void Handle( EventEnvelope<TimePassed> @event )
        {
            var accumulatedTime = database.Get<TotalTimeDetails>( @event.Data.TimeId );

            uint totalTime = 0; 

            if( accumulatedTime != null ) { 
            
                totalTime = accumulatedTime.TotalTime;
            }

            database.Store(
               @event.Data.TimeId,
               @event.Metadata.StreamPosition,
               new TotalTimeDetails(
                   id: @event.Data.TimeId,
                   totalTime: totalTime +  (uint)@event.Data.Time.TotalHours
               )
           );

        }

    }



    





//public class OtherProjection
//{


//    private readonly Database database;
//    public OtherProjections( Database database ) => this.database = database;



//    public void Handle( EventEnvelope<ProductItemAddedToShoppingCart> @event ) =>
//          database.GetAndStore<ShoppingCartDetails>(
//              @event.Data.ShoppingCartId,
//              @event.Metadata.StreamPosition,
//              item =>
//              {
//                  var productItem = @event.Data.ProductItem;
//                  var existingProductItem = item.ProductItems.SingleOrDefault( p => p.ProductId == productItem.ProductId );

//                  if ( existingProductItem == null )
//                  {
//                      item.ProductItems.Add( productItem );
//                  }
//                  else
//                  {
//                      item.ProductItems.Remove( existingProductItem );
//                      item.ProductItems.Add(
//                          new PricedProductItem(
//                              existingProductItem.ProductId,
//                              existingProductItem.Quantity + productItem.Quantity,
//                              existingProductItem.UnitPrice
//                          )
//                      );
//                  }

//                  item.TotalPrice += productItem.TotalAmount;
//                  item.TotalItemsCount += productItem.Quantity;

//                  return item;
//              } );

//    public void Handle( EventEnvelope<ProductItemRemovedFromShoppingCart> @event ) =>
//        database.GetAndStore<ShoppingCartDetails>(
//            @event.Data.ShoppingCartId,
//            @event.Metadata.StreamPosition,
//            item =>
//            {
//                var productItem = @event.Data.ProductItem;
//                var existingProductItem = item.ProductItems.SingleOrDefault( p => p.ProductId == productItem.ProductId );

//                if ( existingProductItem == null || existingProductItem.Quantity - productItem.Quantity < 0 )
//                    // You may consider throwing exception here, depending on your strategy
//                    return item;

//                if ( existingProductItem.Quantity - productItem.Quantity == 0 )
//                {
//                    item.ProductItems.Remove( productItem );
//                }
//                else
//                {
//                    item.ProductItems.Remove( existingProductItem );
//                    item.ProductItems.Add(
//                        new PricedProductItem(
//                            existingProductItem.ProductId,
//                            existingProductItem.Quantity - productItem.Quantity,
//                            existingProductItem.UnitPrice
//                        )
//                    );
//                }

//                item.TotalPrice -= productItem.TotalAmount;
//                item.TotalItemsCount -= productItem.Quantity;

//                return item;
//            } );

//    public void Handle( EventEnvelope<ShoppingCartConfirmed> @event ) =>
//        database.GetAndStore<ShoppingCartDetails>(
//            @event.Data.ShoppingCartId,
//            @event.Metadata.StreamPosition,
//            item =>
//            {
//                item.Status = ShoppingCartStatus.Confirmed;
//                item.ConfirmedAt = DateTime.UtcNow;

//                return item;
//            } );


//    public void Handle( EventEnvelope<ShoppingCartCanceled> @event ) =>
//        database.GetAndStore<ShoppingCartDetails>(
//            @event.Data.ShoppingCartId,
//            @event.Metadata.StreamPosition,
//            item =>
//            {
//                item.Status = ShoppingCartStatus.Canceled;
//                item.CanceledAt = DateTime.UtcNow;

//                return item;

//            }

}
