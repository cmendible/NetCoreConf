FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./webui/webui.csproj", "."]
RUN dotnet restore "webui.csproj"
COPY ./webui/ .
RUN dotnet build "webui.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "webui.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "webui.dll"]