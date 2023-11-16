
# Users API Endpoints

This document describes the functionalities of each endpoint in the `UsersController` of the swe_2_project.

## Endpoints Overview

- `GET /users`
- `POST /users/register`
- `GET /users/verify/{token}`
- `POST /users/login`
- `PUT /users/{id}`
- `DELETE /users/{id}`

### GET /users

- **Description**: Retrieves a list of all registered users.
- **Method**: GET
- **URL Params**: None
- **Success Response**: 
  - Code: 200 OK
  - Content: List of all users
- **Error Response**: None
- **Usage**: To display a list of all users.

### POST /users/register

- **Description**: Registers a new user.
- **Method**: POST
- **Data Params**: 
  - `FirstName`
  - `LastName`
  - `Password`
  - `Email`
  - `Dob` (Date of Birth)
- **Success Response**: 
  - Code: 200 OK
  - Content: `{ Email : user's email }`
- **Error Response**: 
  - Code: 400 Bad Request (e.g., email already in use)
- **Usage**: For new users to register in the system. Sends a verification email to the user.

### GET /users/verify/{token}

- **Description**: Verifies a user's email.
- **Method**: GET
- **URL Params**: `token` (email verification token)
- **Success Response**: 
  - Code: 200 OK
  - Content: `{ message : "Email verified successfully." }`
- **Error Response**: 
  - Code: 400 Bad Request (e.g., invalid token)
- **Usage**: To verify a user's email address using the token sent to them.

### POST /users/login

- **Description**: Authenticates a user and returns an access token.
- **Method**: POST
- **Data Params**: 
  - `email`
  - `password`
- **Success Response**: 
  - Code: 200 OK
  - Content: `{ AccessToken : JWT token }`
- **Error Response**: 
  - Code: 400 Bad Request (e.g., wrong password, email not verified)
- **Usage**: For users to log in to the system. Validates user credentials and provides a JWT token for authenticated sessions.

### PUT /users/{id}

- **Description**: Updates the details of an existing user.
- **Method**: PUT
- **URL Params**: `id` (user's ID)
- **Data Params**: Fields to update (e.g., `FirstName`, `LastName`, `Email`, `Dob`)
- **Success Response**: 
  - Code: 200 OK
  - Content: Updated user details
- **Error Response**: 
  - Code: 404 Not Found (e.g., user not found)
  - Code: 500 Internal Server Error (e.g., database error)
- **Usage**: To allow users or administrators to update user details.

### DELETE /users/{id}

- **Description**: Deletes a user from the system.
- **Method**: DELETE
- **URL Params**: `id` (user's ID)
- **Success Response**: 
  - Code: 204 No Content
- **Error Response**: 
  - Code: 404 Not Found (e.g., user not found)
  - Code: 500 Internal Server Error (e.g., database error)
- **Usage**: To remove a user's record from the system. Typically used by administrators.

## General Information

- Error handling is implemented across all endpoints to provide meaningful error messages and HTTP status codes.
- All endpoints are designed with RESTful principles in mind, ensuring a consistent and predictable API interface.

## Setup Instructions 
- In the appsettings.json, update the values for Token, ConnectionString, EmailUsername, EmailPassword

