# Developer Exercise

## Table of Contents
+ [About](#about)
+ [Project Structure](#project_structure)
+ [Idea](#idea)
+ [Getting Started](#getting_started)
+ [Usage](#usage)

## About <a name = "about"></a>
This is a Web Api project created as a solution to a developer exercise. The idea of this exercise it to create a Grocery Shop Till which can scan fruits and vegetables of different types and produce a numeric result/bill in the end.

## Project Structure <a name = "project_structure"></a>
+ Data - Data Access Layer which includes: Data Models, Context class, Repositories and Data seeding.
+ Services - Service Layer which contains the business logic and also mediates communication between a controller and repository.
+ Tests - Test project including testing of our application.
+ Web - API Layer which is the main Application entry point containing: Controllers, View Models, Middlewares, Startup.
+ Common - Project which cointains commonly shared resources like global constants.

## Idea <a name = "idea"></a>
+ As we can see the project is divided into 3 main parts: Data Access Layer, Service Layer and API Layer.
+ Having implemented the Generic Repository Pattern we create a cleaner solution and seperation of concerns where the Application does not communicate directly with the Data Layer and it also makes it easier to test our code. Controllers are responsible for application flow control logic and the repository is responsible for data access logic. In a situation where we have a lot of models it would be too much if we have to implement a repository for each model, so the generic repository also reduces the amount of code we have to write and thus the possibility of error.
+ The Service Layer is an additional layer that mediates communication between a controller and repository layer. It contains business logic and also our validation logic. 
+ The application uses AutoMapper for object mapping. AutoMapper helps us when we need to flatten complex objects and removes the need to manually map objects. Another good feature is that it works with IQueryable and helps generate optimized SELECT SQL queries.
+ A custom exception handling middleware is implemented in the application. This helps us handle errors throughout our application with the opportunity to create and return our own response depending on what the error is.
+ Swagger is also integrated and it generates for us an easy to use, interactive Api documentation. It makes it easier to test and develop our application.

## Getting Started <a name = "getting_started"></a>
To get a copy of the project up and running on your local machine the only requirement is to change the ConnectionString in you appsettings.json.

## Usage <a name = "usage"></a>
1. The project uses Swagger which upon running generates an Api documentation with a very user friendly interface which shows a list of all controllers defined in our application and provides an easy way to send requests to our API.
2. In our case we have: Deals, Products and Receipts.
3. Initially the project seeds the 2 main deals: `2 for 3` and `buy 1 get 1 half price`.
4. Each controller has the functionality to list a certain amount of objects and get an object by id. For the products we have all CRUD operations supported.
5. Each action has validation to ensure correct input. In case of incorrect input a Client Error response will be returned with information about the error.
6. We first need to create products by sending a POST request to api/products. We can create one or more products. The product object has a name and a price. If we have "price": 50 that means 50 clouds, for "price": 150 we have 1 aws and 50 clouds.
7. Then if we want to add a product to a specific deal we must send a POST request to api/deals/{id}. Again here we can add one or more products.
8. Finally if we want to make a purchase we have to send a POST request to api/receipts with the names of all products we would wish to buy. The response is a receipt object which gives us information about the total price, discount, total price with discount and all products bought.
