# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TransactionAPI/TransactionAPI.csproj", "TransactionAPI/"]
RUN dotnet restore "TransactionAPI/TransactionAPI.csproj"
COPY . .
WORKDIR "/src/TransactionAPI"
RUN dotnet build "TransactionAPI.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "TransactionAPI.csproj" -c Release -o /app/publish

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
USER root
RUN mkdir -p /app/logs && chown $APP_UID /app/logs
USER $APP_UID
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionAPI.dll"]
