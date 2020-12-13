#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["QuartzCore.Blazor/Server/QuartzCore.Blazor.Server.csproj", "QuartzCore.Blazor/Server/"]
COPY ["QuartzCore.Tasks/QuartzCore.Tasks.csproj", "QuartzCore.Tasks/"]
COPY ["QuartzCore.Data.Entity/QuartzCore.Data.Entity.csproj", "QuartzCore.Data.Entity/"]
COPY ["QuartzCore.IService/QuartzCore.IService.csproj", "QuartzCore.IService/"]
COPY ["QuartzCore.Blazor/Shared/QuartzCore.Blazor.Shared.csproj", "QuartzCore.Blazor/Shared/"]
COPY ["QuartzCore.Common/QuartzCore.Common.csproj", "QuartzCore.Common/"]
COPY ["QuartzCore.Data.Freesql/QuartzCore.Data.Freesql.csproj", "QuartzCore.Data.Freesql/"]
COPY ["QuartzCore.Service/QuartzCore.Service.csproj", "QuartzCore.Service/"]
COPY ["QuartzCore.Blazor/Client/QuartzCore.Blazor.Client.csproj", "QuartzCore.Blazor/Client/"]
RUN dotnet restore "QuartzCore.Blazor/Server/QuartzCore.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/QuartzCore.Blazor/Server"
RUN dotnet build "QuartzCore.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuartzCore.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuartzCore.Blazor.Server.dll"]