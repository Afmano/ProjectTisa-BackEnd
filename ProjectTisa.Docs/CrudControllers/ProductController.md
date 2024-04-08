# [ProductController](../../ProjectTisa/Controllers/BusinessControllers/CrudControllers/ProductController.cs)
CRUD controller for [Product](../../ProjectTisa/Models/BusinessLogic/Product.cs).
> [!NOTE]
> **Some** methods require Authorization header with `Manage` policy.
## `Get` 
Return collection of `Products`.
#### Potential responses:
* `200 Ok`
### Success example result:
Content type: `application/json`
```json
[
 {
    "id": 1,
    "name": "Test Product",
    "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
    "description": "desc",
    "price": 111,
    "isAvailable": true,
    "tags": [
      "tag"
    ],
    "discount": null,
    "category": {
      "id": 2,
      "name": "Test Category",
      "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
      "parentCategory": { },
      "subCategories": [],
      "editInfo": { }
    },
    "editInfo": {
        "creationTime": "2024-03-29T18:14:06.606743Z",
        "createdBy": "alpritor",
        "modificationTime": "2024-03-29T18:14:06.606743Z",
        "modifiedBy": "alpritor"
    }
  },
]
```
## `Get` /:id
Return `Product` by id.
#### Potential responses:
* `200 Ok`
* `404 NotFound`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
 {
    "id": 1,
    "name": "Test Product",
    "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
    "description": "desc",
    "price": 111,
    "isAvailable": true,
    "tags": [
      "tag"
    ],
    "discount": null,
    "category": {
      "id": 2,
      "name": "Test Category",
      "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
      "parentCategory": { },
      "subCategories": [],
      "editInfo": { }
    },
    "editInfo": {
        "creationTime": "2024-03-29T18:14:06.606743Z",
        "createdBy": "alpritor",
        "modificationTime": "2024-03-29T18:14:06.606743Z",
        "modifiedBy": "alpritor"
    }
  }
```
## `Get` GetAllByCategory
Return `Product` by [Category](../../ProjectTisa/Models/BusinessLogic/Category.cs).`id`.
#### Potential responses:
* `200 Ok`
### Success example result:
Content type: `application/json`
```json
[
 {
    "id": 1,
    "name": "Test Product",
    "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
    "description": "desc",
    "price": 111,
    "isAvailable": true,
    "tags": [
      "tag"
    ],
    "discount": null,
    "category": {
      "id": 2,
      "name": "Test Category",
      "photoPath": "https://ipfs.io/ipfs/Qmdkkk3hhT9dPngyaGQi1ppGLhcWfXoH8RFB22VLyv7K8P",
      "parentCategory": { },
      "subCategories": [],
      "editInfo": { }
    },
    "editInfo": {
        "creationTime": "2024-03-29T18:14:06.606743Z",
        "createdBy": "alpritor",
        "modificationTime": "2024-03-29T18:14:06.606743Z",
        "modifiedBy": "alpritor"
    }
  },
]
```
## `Post` 
Create `Product` by [ProductCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/ProductCreationReq.cs).
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
Delete `Product` by id.
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
Update `Product` by id with [ProductCreationReq](../../ProjectTisa/Controllers/GeneralData/Requests/CreationReq/ProductCreationReq.cs).
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