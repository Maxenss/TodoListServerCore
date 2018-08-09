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

**Create Todo List**
----
  Post method for add new Todo List on server.

* **URL**

   api/todolists/

* **Method:**
  
  `POST`
  
*  **URL Params**

   Method not have url params.

* **Headers Params**

   Content-Type: application/json
   Authorization: Bearer :token

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 201 Created<br />
    **Content:** `{
    "id": 7110,
    "userId": 2090,
    "title": "List7",
    "tasks": []
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Title is empty.`
    
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Title cannot to be empty.`
        
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User not found.`
            
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: This Todo List already exist.`

* **Request JSON Example:**

  `{
	"title":"New List TItle"
   }`

**Get Todo List**
----
  Get method for get json with Todo List.

* **URL**

   api/todolists/:listId

* **Method:**
  
  `GET`
  
*  **URL Params**

   ListId - ID of Todo List

* **Headers Params**

   Content-Type: application/json
   Authorization: Bearer :token

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `{
    "id": 7110,
    "userId": 2090,
    "title": "List7",
    "tasks": []
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: List id cannot be negative.`
    
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User with this id not found.`
            
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: Todo List with this id not found.`
  

**Get Todo Lists for user**
----
  Get method for get all todo lists in json for user.

* **URL**

   api/todolists/

* **Method:**
  
  `GET`

* **Headers Params**

   Content-Type: application/json
   Authorization: Bearer :token

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `[
    {
        "id": 2,
        "userId": 2,
        "title": "List1",
        "tasks": []
    },
    {
        "id": 1002,
        "userId": 2,
        "title": "List2",
        "tasks": []
    }
]`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User with this id not found.`

**Set Todo List Title**
----
  Patch method for set list's title.

* **URL**

   api/todolists/setlisttitle?listid=:listid&title=:title

* **Method:**
  
  `PATCH`

  *  **URL Params**

   - listId - ID of Todo List
   - title - new title for Todo List

* **Headers Params**

   Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `
    {
        "id": 2,
        "userId": 2,
        "title": "List1",
        "tasks": []
    }`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: List id cannot be negative.`
    
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Title cannot be empty.`
    
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User with this id not found.`
        
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: Todo List with this id not found.`

**Delete Todo List**
----
  Delete method for delete Todo List from server.

* **URL**

   api/todolists/:listId

* **Method:**
  
  `DELETE`

  *  **URL Params**

   listId - ID of Todo List

* **Headers Params**

   - Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `Todo List has been deleted.`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: List id cannot be negative.`
  
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User with this id not found.`
        
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: Todo List with this id not found.`

**Create Todo Task**
----
  Post method for add new Todo Task on server.

* **URL**

   api/todotasks/

* **Method:**
  
  `POST`
  
*  **URL Params**

   Method not have url params.

* **Headers Params**

   Content-Type: application/json
   Authorization: Bearer :token

* **Data Params**

   application/json

* **Success Response:**

  * **Code:** 201 Created<br />
    **Content:** `{
    "id": 2155,
    "toDoListId": 7110,
    "title": "Task2",
    "description": "Need to clear space on windows",
    "taskStatus": "AWAIT"
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Todo list id cannot be negative.`
    
  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Todo task title cannot be empty.`
        
  OR

  * **Code:** 400 Not Found <br />
    **Content:** `Error: Todo task description cannot be empty.`
            
  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User with this id not found.`

* **Request JSON Example:**

  `{
	"ToDoListId":7110,
	"Title":"Task2",
	"Description":"Need to clear space on windows"
}`

**Set Todo Task Status**
----
  Patch method for set status of Todo Task.

* **URL**

   api/:taskId/setstatus/:status

* **Method:**
  
  `PATCH`
  
*  **URL Params**

   taskId - Id of Todo Task
   status - new status from Todo Task

* **Headers Params**

   Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `{
    "id": 2155,
    "toDoListId": 7110,
    "title": "Task2",
    "description": "Need to clear space on windows",
    "taskStatus": "AWAIT"
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Task id cannot be negative.`

  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: User with id cannot be negative.`

  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: Todo Task with this id not found.`
    
  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: Todo List with this id not found.`



**Update Task**
----
  Put method for update Todo Task.

* **URL**

   api/todotasks

* **Method:**
  
  `PUT`

* **Headers Params**

   Authorization: Bearer :token
   Content-Type: application/json

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `{
    "id": 2155,
    "toDoListId": 7110,
    "title": "Task2",
    "description": "Need to clear space on windows",
    "taskStatus": "AWAIT"
}`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Model state is not valid.`

  OR

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Task id cannot be an empty.`

  OR

  * **Code:** 400 Bad Request<br />
    **Content:** `Error: Task title cannot be an empty.`

  OR

  * **Code:** 400 Bad Request<br />
    **Content:** `Error: Task description cannot be an empty.`
        
  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: User with this id not found.`
       
  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: Todo task with this id not found.`

  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: Todo List with this id not found.`

* **JSON request Example**

  `{
    "id": 2,
    "toDoListId": 1002,
    "title": "Task2",
    "description": "Need to clear space on linux",
    "taskStatus": "AWAIT"
   }`


**Delete Task**
----
  Delete method for delete Todo Task.

* **URL**

   api/todotasks/:id

* **Method:**
  
  `DELETE`

* **Headers Params**

   Authorization: Bearer :token

* **Success Response:**

  * **Code:** 200 Ok<br />
    **Content:** `Todo Task has been deleted.`
 
* **Error Response:**

  * **Code:** 400 Bad Request <br />
    **Content:** `Error: Model state is not valid.`

  OR

  * **Code:** 404 Not Found <br />
    **Content:** `Error: User not found.`

  OR

  * **Code:** 404 Not Found<br />
    **Content:** `Error: Todo Task with this id not found.`