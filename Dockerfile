FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["agricultureAPI.csproj", "."]
RUN dotnet restore "./agricultureAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "agricultureAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "agricultureAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "agricultureAPI.dll"]