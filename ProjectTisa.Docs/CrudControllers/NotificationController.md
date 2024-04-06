# [NotificationController](../../ProjectTisa/Controllers/BusinessControllers/CrudControllers/NotificationController.cs)
CRUD controller for [Notification](../../ProjectTisa/Models/BusinessLogic/Notification.cs).
> [!IMPORTANT]
> Require Authorization header. **Some** methods only by `Manage` policy
## `Get` 
Return collection of `Notifications`.
#### Potential responses:
* `200 Ok`
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
[
 {
    "id": 1,
    "caption": "Notif caption",
    "message": "Some message, hello!",
    "notificationType": "None",
    "editInfo": {
      "creationTime": "2024-04-02T09:11:38.77613Z",
      "createdBy": "alpritor",
      "modificationTime": "2024-04-02T17:00:38.357759Z",
      "modifiedBy": "alpritor"
    }
  },
]
```
## `Get` /:id
Return `Notification` by id.
#### Potential responses:
* `200 Ok`
* `401 Unauthorized`
* `403 Forbidden`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```
{
    "id": 1,
    "caption": "Notif caption",
    "message": "Some message, hello!",
    "notificationType": "None",
    "editInfo": {
      "creationTime": "2024-04-02T09:11:38.77613Z",
      "createdBy": "alpritor",
      "modificationTime": "2024-04-02T17:00:38.357759Z",
      "modifiedBy": "alpritor"
    }
}
```
## `Post` 
Create `Notification` by [NotificationCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/NotificationCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
{
  "message": "Request handled successfully."
}
```
## `Delete` /:id
Delete `Notification` by id.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```
{
  "message": "Request handled successfully."
}
```
## `Put` /:id
Update `Notification` by id with [NotificationCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/NotificationCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```
{
  "message": "Request handled successfully."
}
```