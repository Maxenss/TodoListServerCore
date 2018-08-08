**User Sign Up**
----
  Post method for add new user on server.

* **URL**

   api/account/signup

* **Method:**
  
  `POST`
  
*  **URL Params**

   Method not have url params.

* **Headers Params**

   Content-Type

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 201 Created<br />
    **Content:** `{
    "id": 2091,
    "name": "New User",
    "email": "newuser@gmail.com",
    "password": "123456",
    "todoLists": []
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Empty Email.`
    
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Email is not valid.`
        
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Empty Password`
            
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Short Password`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Empty Name`
        
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `User with this email already exist.`
          
* **Request JSON Example:**

  `{
	"email":"newuser@gmail.com",
	"password":"123456",
	"name":"New User"
}`


**User Sign In**
----
  Post method for authorization user on server.

* **URL**

   api/account/signin

* **Method:**
  
  `POST`
  
*  **URL Params**

   Method not have url params.

* **Headers Params**

   Content-Type

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIwOTAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNTMzODU2MjMyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDQ1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA0NSJ9.fHjx_ANgziWMg99I7xL4XhBVcBPOQ7J2v_iXwJKSu4Q",
    "tokenExpires": "2018-08-10T02:10:32.7169668+03:00",
    "responseTime": "2018-08-09T02:10:32.7169675+03:00",
    "user": {
        "id": 2090,
        "name": "Name",
        "email": "user@gmail.com",
        "password": "123456",
        "todoLists": []
    }
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Empty Email.`
    
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Email is not valid.`
        
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Empty Password.`
            
  OR

  * **Code:** 404 Bad Request <br />
    **Content:** `Error: Not correct email or password.`

* **Request JSON Example:**

  `{
	"email":"user@gmail.com",
	"password":"123456"
}`


**Delete User**
----
  Delete method for remove user from server.

* **URL**

   api/users

* **Method:**
  
  `DELETE`
  
* **Headers Params**

   Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `User has been deleted.`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User not found.`

**Get User**
----
  Get json with user from server.

* **URL**

   api/users/:id

* **Method:**
  
  `GET`
  
* **Headers Params**

   Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `{
    "id": 2092,
    "name": "New User",
    "email": "newuser2@gmail.com",
    "password": "123456",
    "todoLists": []
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid`

  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User not found.`
  