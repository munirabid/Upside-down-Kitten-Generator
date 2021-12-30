## General info
* This project is to submit assement. 
* It is mainly focused to utilise given cat image API 
* Implement some endpoint using different authentications.
* Basic Authentication and JWT authentication.
	
## Technologies

* Project is created with ASP.NET Core API 3.1.
* I used visual studio 2019 to develope, build and run this solution.
* Service level implementation is followed.
* Dependency Injection is implemented to avoid tight coupling.

## Usage
* You can consume GetRandomCat under CatController to get random image of cat. It does not require any authentication.
* Consume GetUsers and getuser endpoints under UsersController to use JWT Authentication.
* GetUserById endpoint uses basic authentication. you need to send username and password in Authorization header.
* Use Authenticate endpoint under user controller to generate JWT token and use this token in JWT Authentication methods.
* consume Register user endpoint to register a user. 

## Flow
* you can test GetRandomCat without any data. just send request and you will get image in return.

To test other methods follow below steps.

* First of all register a user by providing necessary information.
* once user is registered you will have data to authenticate user.
* you can test GetUserById method by providing username and password with basic authentication.
* now authenticate user by providing username and password. in return you will get JWT token.
* Use JWT authentication to test GetUsers and GetUser. Provide jwt token in authroze form that is provided by swagger.

## Setup
To run this project simple run the project and it will open a browser page with swagger interface.
Please chage the RealworldOneBackendTest.xml path according to your directory. 
Follow the steps below
* Right click on project. Then go to properties.
* Select Build section.
* Change the path of XML documentation File  according to your local directory. 
