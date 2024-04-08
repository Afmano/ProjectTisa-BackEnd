# [AuthController](../../ProjectTisa/Controllers/BusinessControllers/UserRelatedControllers/AuthController.cs)

Controller for authorization. Using **Bearer JWT token** as auth method.
## `POST` Authorize
Return [JWT token](https://en.wikipedia.org/wiki/JSON_Web_Token) by passing [login data](../../ProjectTisa/Controllers/GeneralData/Requests/UserReq/UserLoginReq.cs).  
#### Potential responses:
* `200 Ok`<br>[TokenResponse](../../ProjectTisa/Controllers/GeneralData/Responses/TokenResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTb21lUm9sZSIsImV4cCI6MTcxMjU3MDAwMCwiaXNzIjoid3d3LnRpc2EtYmFja2VuZC5jb20iLCJhdWQiOiJ3d3cudGlzYS1mcm9udGVuZC5jb20ifQ.PRXhoRT4AQTTwBwzfuVD_CtTSIYpZxPHwCWK1Atz2xVAWi1ZAuGNp-s4CSMyZtoyEezgWAhkQoLbekcYSBUqfA",
  "exparationDate": "2024-04-05T14:38:20.230Z"
}
```
## `GET` CheckIsEmailExist
Check is email exist in `User` table at current context.<br>Return boolean.
#### Potential responses:
* `200 Ok`<br>[BooleanResponse](../../ProjectTisa/Controllers/GeneralData/Responses/BooleanResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "result": true
}
```
## `GET` CheckIsUsernameExist
Check is username exist in `User` table at current context.<br>Return boolean.
#### Potential responses:
* `200 Ok`<br>[BooleanResponse](../../ProjectTisa/Controllers/GeneralData/Responses/BooleanResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "result": true
}
```
## `POST` Registrate
Registrate new user at database's `PendingRegistration`, using request: [UserInfoReq](../../ProjectTisa/Controllers/GeneralData/Requests/UserReq/UserInfoReq.cs).<br>Return `PendingRegistration` code id.
#### Potential responses:
* `200 Ok`<br>[IdResponse](../../ProjectTisa/Controllers/GeneralData/Responses/IdResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "id": 231
}
```
## `POST` Verify
Verify pending registration request by sending code. If code same in table - create new `User`.<br>Return [JWT token](https://en.wikipedia.org/wiki/JSON_Web_Token).
#### Potential responses:
* `200 Ok`<br>[TokenResponse](../../ProjectTisa/Controllers/GeneralData/Responses/TokenResponse.cs)
* `400 BadRequest`<br>[MessageResponse](../../ProjectTisa/Controllers/GeneralData/Responses/MessageResponse.cs)
### Success example result:
Content type: `application/json`
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTb21lUm9sZSIsImV4cCI6MTcxMjU3MDAwMCwiaXNzIjoid3d3LnRpc2EtYmFja2VuZC5jb20iLCJhdWQiOiJ3d3cudGlzYS1mcm9udGVuZC5jb20ifQ.PRXhoRT4AQTTwBwzfuVD_CtTSIYpZxPHwCWK1Atz2xVAWi1ZAuGNp-s4CSMyZtoyEezgWAhkQoLbekcYSBUqfA",
  "exparationDate": "2024-04-08T14:42:18.9158275Z"
}
```