# [AdminController](../../ProjectTisa/Controllers/BusinessControllers/ManageControllers/AdminController.cs)
Controller with methods for **Admin** role.
> [!IMPORTANT]
> Require Authorization header with `Admin` policy.
## `PATCH` SetRole
Set [role](../../ProjectTisa/Models/Enums/RoleType.cs) to `User`. Can't interact with **Admin+** roles.<br>Return message.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```
{
  "message": "Request handled successfully."
}
```