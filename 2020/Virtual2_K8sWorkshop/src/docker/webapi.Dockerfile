FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./webapi/webapi.csproj", "."]
RUN dotnet restore "webapi.csproj"
COPY ./webapi/ .
RUN dotnet build "webapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "webapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "webapi.dll"]