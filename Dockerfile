#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GraphQLLearning/GraphQLLearning.csproj", "GraphQLLearning/"]
RUN dotnet restore "GraphQLLearning/GraphQLLearning.csproj"
COPY . .
WORKDIR "/src/GraphQLLearning"
RUN dotnet build "GraphQLLearning.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQLLearning.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQLLearning.dll"]