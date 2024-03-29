﻿namespace Cph.DDD.Meetup.Logistics.Domain.Tests;

using Cph.DDD.Meetup.Logistics.Domain.Common;
using Cph.DDD.Meetup.Logistics.Domain.Time;
using FluentAssertions;

public class TimeShould
{
    [Fact]
    public void Pass() =>
        TotalTime.Initial
        .Decide( new PassTime(TimeSpan.FromHours( 1 ) ) )
        .Should()
        .Equal( new TimePassed( Guid.Empty, TimeSpan.FromHours( 1 ) ).Singleton() );

    [Fact]
    public void Accumulate() =>
      new IEvent[]
        {
            new TimePassed(Guid.Empty, TimeSpan.FromHours( 1 )),
            new TimePassed(Guid.Empty, TimeSpan.FromHours( 1 )),
            new TimePassed(Guid.Empty, TimeSpan.FromHours( 1 )),
            new TimePassed(Guid.Empty, TimeSpan.FromHours( 1 )),
            new TimePassed(Guid.Empty, TimeSpan.FromHours( 1 )),
        }
    .Fold( TotalTime.Initial )
        .Should()
        .Be( new TotalTime( Guid.Empty, TimeSpan.FromHours( 5 ) ) );
}