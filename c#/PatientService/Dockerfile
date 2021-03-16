FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /source

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release --no-restore -o out/

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-alpine AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /source/out ./
ENTRYPOINT ["dotnet", "PatientService.dll"]
