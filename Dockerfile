FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./ProjectTisa/ProjectTisa.csproj"
RUN dotnet publish "./ProjectTisa/ProjectTisa.csproj" -c release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "ProjectTisa.dll"]