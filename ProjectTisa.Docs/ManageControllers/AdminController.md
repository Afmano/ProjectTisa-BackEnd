# [AdminController](../../ProjectTisa/Controllers/BusinessControllers/ManageControllers/AdminController.cs)
> [!IMPORTANT]
> Require Authorization header with `Admin` policy.

Controller with methods for **Admin** role.
## SetRole
Set [role](../../ProjectTisa/Models/Enums/RoleType.cs) to `User`. Can't interact with **Admin+** roles.
Return message.
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