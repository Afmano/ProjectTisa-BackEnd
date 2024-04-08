# [DiscountController](../../ProjectTisa/Controllers/BusinessControllers/CrudControllers/DiscountController.cs)
CRUD controller for [Discount](../../ProjectTisa/Models/BusinessLogic/Discount.cs).
> [!NOTE]
> **Some** methods require Authorization header with `Manage` policy.
## `Get` 
Return collection of `Discounts`.
#### Potential responses:
* `200 Ok`
### Success example result:
Content type: `application/json`
```json
[
 {
    "id": 1,
    "name": "Discount",
    "description": "Some desc",
    "discountPercent": 0.45,
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
Return `Discount` by id.
#### Potential responses:
* `200 Ok`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
    "id": 1,
    "name": "Discount",
    "description": "Some desc",
    "discountPercent": 0.45,
    "editInfo": {
      "creationTime": "2024-04-02T09:11:38.77613Z",
      "createdBy": "alpritor",
      "modificationTime": "2024-04-02T17:00:38.357759Z",
      "modifiedBy": "alpritor"
    }
}
```
## `Post` 
Create `Discount` by [DiscountCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/DiscountCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `Delete` /:id
Delete `Discount` by id.
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```
## `Put` /:id
Update `Discount` by id with [DiscountCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/DiscountCreationReq.cs).
#### Potential responses:
* `200 Ok`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
* `400 BadRequest`<br>[ValidationProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails)
* `401 Unauthorized`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "message": "Request handled successfully."
}
```