---
puppeteer:
  landscape: false
  format: "A4"
  timeout: 3000 # <= Special config, which means waitFor 3000 ms
---

# Workshop Solution for simple logistics domain

These solutions are just how we did it.It doesn't mean its the only way to solve the exercises.


@import "assets/simple-logistics-domain.png" {width="600px" height="400px"}
*fig.1. simple logistics domain.*

## Exercise 1.C (4 Persons)

Make sure every one in the team understands the problem. Why will a booking list of A,B,B give a total estimated delivery time of 7 hours?

Truck 1 drives towards  A  after 1 hour it returns; after two hours its back at the factory to pick up  booking B2.
Ferry sails towards A after 1 hour and after 4 extra hour it delivers booking A
Truc2 drives towards B and delivers booking B1 after 5 hours
Truckt 1 delivers booking B2 at B after 2 + 5 = 7 hours


## Exercise 3.A (Alone) 

How would You solve the problem.

One way to solve this is to imagine a kind of simulation where we progress the simulation in steps of one hour.
This could run in a loop until all bookings has been delivered.
each hour
Trucks, Ferry, Factory, Bookings could get its state updated. 
The total number  of hours could be summarized.  

## Exercise 4.A (2 persons)

Write down a list of concepts you can find in the domain. Truck and Ferry etc are obvious candidates. But find good names for concepts you would use in your implemented solution and in the documentation of your solution.

A list ofconcepts could be

{Factory, Booking, Product or Goods, Truck, Driver, Factory Manager, Ferry, Port, Time, Location, Route, Delivery Location, Transport Network, Transport Direction }




## Exercise 4.B (4 persons)

Actors:

Drivers, Captain, Booking manager or planner


## Exercise 5.A write some pseudo code (Alone)

Write some classes or types for the different concepts you found in Exercise 4.A.
You can use pen and paper or write in a text editor.


Suggestions:

Or look at [solution by Jacub Konecki](https://github.com/jkonecki/SoftwarePark/tree/master/TransportTycoon/TransportTycoon)

Network {
  Locations,
  Routes
}

Route{
  Locations
  DistanceInTime
}

Transport Description {
  Delivery Location
  Routes
}

Truck{
  CurrentAssignment: TransportDescripton
  CurrentLocation:
  Direction
  TargetDestination
  IsVacant
}

Ferry{
  CurrentAssignment: TransportDescripton
  CurrentLocation:
  Direction
  TargetDestination
  IsVacant
}


Location {
  Capacity
  Stock
}

Factory:Location

Booking {
  Destination : Location {A|B}
}

Simulation {
 Network
 Trucks
 Ferry
 Bookings
}

## Exercise 6.A Mini Event Storming (2 Persons)

Imagine all the events that happens in the system. 

Try to make a sequence of events of what happens from a list of bookings are received to all goods has been delivered.

E.G. Bookings Received, Truck Assigned to transport, Goods offloaded at port. etc. 

See https://miro.com/app/board/uXjVNlnxyPE=/ for a solution proposal

## Exercise 6.A Mini Event Storming (4 Persons)

Compare your event sequenses all together.




<H1 style="color:red"> Html Title </H1>



## Exercise 5.A (2 persons)





## Video Test



<video controls>
  <source src="assets/Nasa-Mars-Mobile-Android-optimized_preview.mp4" type="video/mp4">
  Your browser does not support the video tag.
</video>









