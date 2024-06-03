# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev

WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "DotNetBaseProject/Alafein.API.csproj"
    
# Copy everything else and build
COPY . .

WORKDIR "/src/DotNetBaseProject"
RUN dotnet publish "Alafein.API.csproj" -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
ADD DotNetBaseProject/Resources /app/Resources

COPY --from=build /app .
ENTRYPOINT ["dotnet", "Alafein.API.dll", "--server.urls", "http://0.0.0.0:5000"]