# [OrderController](../../ProjectTisa/Controllers/BusinessControllers/CrudControllers/OrderController.cs)
CRUD controller for [Order](../../ProjectTisa/Models/BusinessLogic/Order.cs).
> [!IMPORTANT]
> Require Authorization header. **Some** methods only by `Manage` policy.
## `Get` 
Return collection of `Orders`.
#### Potential responses:
* `200 Ok`
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```json
[
 {
    "id": 1,
    "user": { },
    "updateNote": "Some note",
    "status": "InProgress",
    "totalPrice": 111,
    "editInfo": {
      "creationTime": "2024-04-06T15:26:31.020Z",
      "createdBy": "string",
      "modificationTime": "2024-04-06T15:26:31.020Z",
      "modifiedBy": "string"
    },
    "productQuantities": [{},]
 },
]
```
## `Get` /:id
Return `Order` by id.
#### Potential responses:
* `200 Ok`
* `401 Unauthorized`
* `403 Forbidden`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
    "id": 1,
    "user": { },
    "updateNote": "Some note",
    "status": "InProgress",
    "totalPrice": 111,
    "editInfo": {
      "creationTime": "2024-04-06T15:26:31.020Z",
      "createdBy": "string",
      "modificationTime": "2024-04-06T15:26:31.020Z",
      "modifiedBy": "string"
    },
    "productQuantities": [{},]
}
```
## `Post` 
Create `Order` by [OrderCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/OrderCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `403 Forbidden`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `Put` /:id
Update `Order` by id with [OrderCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/OrderCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `Patch` CompleteOrder
Set `Order` status to `Completed` by id.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `Patch` CancelOrder
Set `Order` status to `Cancelled` by id.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `403 Forbidden`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```