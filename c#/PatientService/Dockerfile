FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release --no-restore -o out/

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /source/out ./
ENTRYPOINT ["dotnet", "PatientService.dll"]
