# [ManagerController](../../ProjectTisa/Controllers/BusinessControllers/ManageControllers/ManagerController.cs)
Controller to interact with `User` data.
> [!IMPORTANT]
> Require Authorization header with `Manage` policy.
## `PATCH` SendNotificationByRole
Send `Notification` for `Users` by [role](../../ProjectTisa/Models/Enums/RoleType.cs).<br>Return message.
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
## `PATCH` SendNotificationByUsername
Send `Notification` for `User` by username.<br>Return message.
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
## `PATCH` SendNotificationByEmail
Send `Notification` for `User` by email.<br>Return message.
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
## `PATCH` DetachNotification
Detach all `Users` from selected `Notification`.<br>Return message.
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
## File section
>[!NOTE]
> Currently using [Pinata](https://www.pinata.cloud/) service.
## `POST` LoadFile
Load file to server external storage.<br>Return **IpfsHash** with info.
#### Potential responses:
* `200 Ok`
* `400 BadRequest`
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
{
  "IpfsHash": "Qmetf8kQ2Lvoe31S36cxcvuPhysET35cQGhEcVPJDHSjT7",
  "PinSize": 71057,
  "Timestamp": "2024-04-05T11:19:51.122Z"
}
```
## `GET` GetFiles
Get list of files from external storage.<br>Return message.
#### Potential responses:
* `200 Ok`
* `400 BadRequest`
* `401 Unauthorized`
### Success example result:
Content type: `application/json`
```
{
  "count": 1,
  "rows": [
    {
      "id": "jk8djd3u-4f3b-qedv-99ec-y7n403eb2yrg",
      "ipfs_pin_hash": "Qmetf8kQ2Lvoe31S36cxcvuPhysET35cQGhEcVPJDHSjT7",
      "size": 71057,
      "user_id": "vtncz2no-6c41-4eab-bqhl-zvt7jlwgc6op",
      "date_pinned": "2024-04-05T11:19:51.122Z",
      "date_unpinned": null,
      "metadata": {
        "name": "1.jpg",
        "keyvalues": null
      },
      "regions": [
        {
          "regionId": "pinata",
          "currentReplicationCount": 1,
          "desiredReplicationCount": 1
        }
      ],
      "mime_type": "image/jpeg",
      "number_of_files": 1
    }
  ]
}
```