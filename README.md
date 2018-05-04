# Live_Project_Snippets
Code snippets that I have completed from tickets while on a two week live project.


This live project was about developing a web application that could register new employees, keep track of their details such as department and salary, as well as when they clocked in and out of their shift. 
I was responsible for the clocking in and out portion.  I created a controller to handle all the functions necessary as well as all the views pertaining to what the user would be able to see.


On the login page, I created a clock in/out button.  When the employee types in their username, I used Ajax and JSON to check the database to make sure the employee is registered and that they entered the correct password by using the password hasher.  If so, it would also check the events for that day of that employee and determine if they need to clock in or clock out and the button would display accordingly as well as display a message of confirmation that it was successful.
Then if the employee wanted to log into their account, I directed them to a view that showed a list of all their clock in and out times and days as well as how long they worked for that shift.


On the flip side, if an admin logged in, they were able to see all the employees.  When they clicked the detail button for a specific employee, I created a modal that showed a partial view of that employee’s personal information such as address, department, title, and whatnot.
