**User Sign Up**
----
  Post method for add new user on server.

* **URL**

/users/signup

* **Method:**
  
  `GET`
  
*  **URL Params**

   Method not have url params.

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
          
* **Request Data Example:**

 `{
	"email":"newuser@gmail.com",
	"password":"123456",
	"name":"New User"
}`

* **Notes:**

  <_This is where all uncertainties, commentary, discussion etc. can go. I recommend timestamping and identifying oneself when leaving comments here._> 
