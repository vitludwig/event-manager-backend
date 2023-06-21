FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eventapp.csproj", "."]
RUN dotnet restore "eventapp.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "eventapp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eventapp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY [".env", "."]
COPY --from=publish /app/publish .

# Now we cannot use mode from .env...
# https://youtrack.jetbrains.com/issue/RIDER-57241
#ENTRYPOINT ["dotnet", "payment.dll", "--environment=Development"]

ENTRYPOINT ["dotnet", "eventapp.dll"]
