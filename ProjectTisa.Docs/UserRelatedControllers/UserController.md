# [UserController](../ProjectTisa/Controllers/BusinessControllers/UserRelatedControllers/UserController.cs)
> [!IMPORTANT]
> Require Authorization header.

Controller to interact with `User` data.
## GetUser
Return current `User`.
#### Potential responses:
* `200 Ok` 
[User](../../ProjectTisa/Models/User.cs)
* `400 BadRequest` 
[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
{
  "id": 0,
  "username": "string",
  "email": "string",
  "registrationDate": "2024-04-05T10:47:58.497Z",
  "lastSeen": "2024-04-05T10:47:58.497Z",
  "role": "User"
}
```
## ChangePassword
Change `User's` password using same salt.
Return boolean.
#### Potential responses:
* `200 Ok` 
[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest` 
[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
{
  "message": "Request handled successfully."
}
```
## ChangeEmail
> [!CAUTION]
> NOT IMPLEMENTED YET.