#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8897

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["V9ManagerIVR/V9ManagerIVR.csproj", "V9ManagerIVR/"]
COPY ["V9Common/V9Common.csproj", "V9Common/"]
RUN dotnet restore "V9ManagerIVR/V9ManagerIVR.csproj"
COPY . .
WORKDIR "/src/V9ManagerIVR"
RUN dotnet build "V9ManagerIVR.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "V9ManagerIVR.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "V9ManagerIVR.dll"]