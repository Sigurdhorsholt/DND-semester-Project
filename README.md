# DND-semester-Project
Shared repo for .net development course project - Group work.
Usernames:
- Ceciliejs
- Sigurdhorsholt
- Jonasbj8
- Rasm105j


# 1. Blogpost - Project Formulation & Requirements



## Shared Laundrymat Online booking system

####  Description

General idea is a system that allows users in an apartement complex with shared laundry system in the complex to book slots from their couch instead of going to the basement. 

The value proposition is that it sucks to go to the basement twice. First to book the slot and then later to do the laundry. You might even get 3 trips if no times are available.

**The system**
**Backend:**
- Multible users
-- SysAdmin for adding new apartement complexes and adding complex admins
-- Complex Admin for adding time slots, machines and users for his/hers complex
-- Daily users - can only manage their own password and book/delete time slots.

- Laundry room
-- laundry room entity
-- Laundry machine entity
-- Drying machine entity

- Time entities
-- Booking - Complex admin can vary length of booking slots for indiviudal complex
-- Bookings will be available to users as time slots and can be available/unavailable
-- Bookings cannot overlap an should be locked
-- When a user is booking the time slot should be locked for other users UNTIL the user confirms his/hers booking. This needs a locked functionlaity in DB that will be dependent on the users confirmation
-- A systemAdmin can configure how many bookings each user is allowed to have 


- Config site backend
-- Admin config site to setup: bookings, timeslots, number of allowed bookings, Number of available machines etc.

- Auth system
-- Secure Login feature
-- Password reset system (maybe through email)
-- Users should be registered through accept of the complex admin
-- Users should be allowed to change email and username but should be locked to an appartment number that users themselves cannot change. This way the admin doesnt need to help when new people move into appartment and the account needs to change hands...

**FrontEnd :**

- Log in page
-- Register / forgot password / login
- Homepage
-- This is where users current bookings are visible
-- Make new booking
-- Should generate based on how complex admin have configured complex
-- Available and booked time slots. 
-- Wether users can see Which other apartment has booked a time slots should be set in config for privacy reasons. 
- add / delete booking 
- Manage user page
--  Change: username, password, email.

**User Stories**

Daily Users:
*"As a user, I want to login ti the system so that I can access the laundy booking system"*

