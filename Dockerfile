FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . .
RUN dotnet restore ./AspNetCoreOnDocker.Api/

# Copy everything else and build
WORKDIR /app/AspNetCoreOnDocker.Api/
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/AspNetCoreOnDocker.Api/out .
ENTRYPOINT ["dotnet", "AspNetCoreOnDocker.Api.dll"]