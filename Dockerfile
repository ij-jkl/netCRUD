# Saca la imagen del SDK del .net que usemos, en este caso 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo .csproj y restaura las dependencia
COPY Crud-API.csproj ./Crud-API/
RUN dotnet restore "Crud-API/Crud-API.csproj"

# Copy todo
COPY . ./Crud-API/

# Se construye
WORKDIR /src/Crud-API
RUN dotnet build "Crud-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crud-API.csproj" -c Release -o /app/publish


# Permite usar Curl para debugear desde Docker
RUN apt-get update && apt-get install -y curl

1
# Postea la img final del Project
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crud-API.dll"]