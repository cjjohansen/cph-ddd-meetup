---
puppeteer:
  landscape: false
  format: "A4"
  timeout: 3000 # <= Special config, which means waitFor 3000 ms
---

# Workshop

Wellcome to this workshop. We hope you will learn alot and that you will find that your time has been well spent.

## Problem Domain

This workshop gets it inspiration from the workshop from Trustbits gihub repository which can be found [here:](https://github.com/trustbit/exercises/blob/master/transport-tycoon-1.md) 

Below is a simple drawing of the problem domain we are going to use for our workshop.

Imagine that we receive a list of orders for delivery of goods produced at the **Factory** to a delivery location which can be either **A** or **B**.

The numbers 1,4 and 5 denotes the transportation time 1 and 5 in hours for transport by truck from factory to Port and Location B respectively.

In this scenario there are two trucks at our disposal and 1. Ferry.

We want to calculate estimated delivery time for delivering goods.

@import "assets/simple-logistics-domain.png" {width="600px" height="400px"}
*fig.1. simple logistics domain.*

Given we have a list of bookings with only a one single booking  for delivery at location A.
Then the total time for delivering all orders would be 5 as shown in fig. 1.
Naturally since it would take a truck 1 hour to deliver the goods at the Port and then 4 hours for the ferry to transport from Port to Location A.

for a list of order [A, B] the total time would still be 5 as the 2nd truck could bring goods to Location B simultaneously and that would also take 5 hours.

As the booing list gets longer it gets more and more  difficult to calculate what the estimated time will be.

!!! note Notice
    
    - Return travel time needs to be taken into account for both trucks and the Ferry.
    - Trucks and ferry can move forth and back with no delay or breaks. 




## Introduction to exercises

We want this to be a collaborativ workshop. We use the 1,2,4 technique from Liberating Structures a lot.

We will deal out cards form a standard deck of cards. All persons that draw a 3 will form a team and those who draw 9 will form a team and so forth we will have many teams of 4 people.

In the following exercises you will alternate beteen being 1,2 or 4 people solving an exercise.


## Exercise 1.A (4 persons)

Find your team members. 

## Exercise 1.B (4 Persons)

Find a place where you can think. Sitting at the same table makes sense.

## Exercise 1.C (4 Persons)

Make sure every one in the team understands the problem. Why will a booking list of A,B,B give a total estimated delivery time of 7 hours?


## Exercise 3.A (Alone) 

Find paper and a pen. Take some time 1 by 1 to think about the problem! How would you solve it using your favorite programmming language. Take notes to help you explain your thoughts to your team mates.

## Exercise 3.B (2 Persons)

Team up with one other person in your 4 people team. And share what your thoughts on how you will solve the problem. Make sure you understand the other persons explanations.

## Exercise 3.C (4 Persons)

Merge your two 2 person teams and up with one other person in your 4 people team. And share what your thoughts on how you will solve the problem.

Make sure you all understand each others ideas. See if you can align on what you would do as a team.

## Exercise 4.A (2 persons)

Write down a list of concepts you can find in the domain. Truck and Ferry etc are obvious candidates. But find good names for concepts you would use in your implemented solution and in the documentation of your solution.

## Exercise 4.B (4 persons)

Compare your lists with the other sub team.

Aggree on a combined list.

Also make a seperate list of actors or persons that would be involved in the system.

(We dont have self driving cars yet in this scenario).

Strike a balance don't spent to much time. This is an exercise we dont want to complicate things. But in a real world scenario. This step is important.


## Exercise 5.A write some pseudo code (Alone)

Write some classes or types for the different concepts you found in Exercise 4.B.
You can use pen and paper or write in a text editor.

Add properties to the concepts.




## Exercise 6.A Mini Event Storming (2 Persons)

Imagine all the events that happens in the system. 

Try to make a sequence of events of what happens from a list of bookings are received to all goods has been delivered.

E.G. Bookings Received, Truck Assigned to transport, Goods offloaded at port. etc. 

## Exercise 6.A Mini Event Storming (4 Persons)

Compare your event sequenses all together.




<H1 style="color:red"> Html Title </H1>



## Exercise 5.A (2 persons)





## Video Test



<video controls>
  <source src="assets/Nasa-Mars-Mobile-Android-optimized_preview.mp4" type="video/mp4">
  Your browser does not support the video tag.
</video>









