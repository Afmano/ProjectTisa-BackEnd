# [CategoryController](../../ProjectTisa/Controllers/BusinessControllers/CrudControllers/CategoryController.cs)
CRUD controller for [Category](../../ProjectTisa/Models/BusinessLogic/Category.cs).
> [!NOTE]
> **Some** methods require Authorization header with `Manage` policy.
## `Get` 
Return collection of `Categories`.
#### Potential responses:
* `200 Ok`
### Success example result:
Content type: `application/json`
```json
[
 {
    "id": 1,
    "name": "Category",
    "photoPath": "https://pink-raw-gibbon-672.mypinata.cloud/ipfs/QmPvZ22diyYgGMHd7xekdnQe3UBpzn15RhmDoKU3WQWj5E",
    "parentCategory": null,
    "subCategories": [],
    "editInfo": {
      "creationTime": "2024-03-28T12:19:56.708452Z",
      "createdBy": "alpritor",
      "modificationTime": "2024-03-28T12:19:56.708452Z",
      "modifiedBy": "alpritor"
    }
  },
]
```
## `Get` /:id
Return `Category` by id.
#### Potential responses:
* `200 Ok`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
 {
    "id": 1,
    "name": "Category",
    "photoPath": "https://pink-raw-gibbon-672.mypinata.cloud/ipfs/QmPvZ22diyYgGMHd7xekdnQe3UBpzn15RhmDoKU3WQWj5E",
    "parentCategory": null,
    "subCategories": [],
    "editInfo": {
      "creationTime": "2024-03-28T12:19:56.708452Z",
      "createdBy": "alpritor",
      "modificationTime": "2024-03-28T12:19:56.708452Z",
      "modifiedBy": "alpritor"
    }
  }
```
## `Post` 
Create `Category` by [CategoryCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/CategoryCreationReq.cs).
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
## `Delete` /:id
Delete `Category` by id.
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
Update `Category` by id with [CategoryCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/CategoryCreationReq.cs).
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