namespace Cph.DDD.Meetup.Logistics.Domain.Tests;

using Cph.DDD.Meetup.Logistics.Domain;
using FluentAssertions;
using System.Runtime.InteropServices;

public class AccountShould
{
    [Fact]
    public void Make_a_deposit() =>
        State.Initial
        .Decide( new Deposit( 5, DateTime.MinValue ) )
        .Should()
        .Equal( new Deposited( new( 5, DateTime.MinValue ) ).Singleton() );

    [Fact]
    public void Make_a_withdrawal() =>
        State.Initial
        .Decide( new Withdraw( 5, DateTime.MinValue ) )
        .Should()
        .Equal( new Withdrawn( new( 5, DateTime.MinValue ) ).Singleton() );

    [Fact]
    public void Close_the_account_and_withdraw_the_remaining_amount() =>
        new IEvent[]
        {
            new Deposited(new(5, DateTime.MinValue)),
            new Deposited(new(5, DateTime.MinValue))
        }
    .Fold()
        .Decide( new Close( DateTime.MinValue ) )
        .Should()
        .Equal( new IEvent[]
        {
            new Withdrawn(new(10, DateTime.MinValue)),
            new Closed(DateTime.MinValue)
        } );
}