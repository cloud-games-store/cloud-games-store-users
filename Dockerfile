FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore src/Users.API/*.csproj

WORKDIR /app/src/Users.API

RUN dotnet publish -c Release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /dist

COPY --from=build /dist ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Users.API.dll"]
