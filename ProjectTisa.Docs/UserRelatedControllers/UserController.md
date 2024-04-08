# [UserController](../../ProjectTisa/Controllers/BusinessControllers/UserRelatedControllers/UserController.cs)
> [!IMPORTANT]
> Require Authorization header.

Controller to interact with `User` data.
## `GET` GetUser
Return current `User`.
#### Potential responses:
* `200 Ok`<br>[User](../../ProjectTisa/Models/User.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```json
{
  "id": 0,
  "username": "string",
  "email": "string",
  "registrationDate": "2024-04-05T10:47:58.497Z",
  "lastSeen": "2024-04-05T10:47:58.497Z",
  "role": "User"
}
```
## `PATCH` ChangePassword
Change `User's` password using same salt.<br>Return message.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `PATCH` ChangeEmail
> [!CAUTION]
> NOT IMPLEMENTED YET.