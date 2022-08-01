#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Discount/Discount.api/Discount.api.csproj", "Services/Discount/Discount.api/"]
RUN dotnet restore "Services/Discount/Discount.api/Discount.api.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Discount.api"
RUN dotnet build "Discount.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.api.dll"]