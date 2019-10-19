FROM mcr.microsoft.com/dotnet/core/runtime:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY SupportApplications/PaymentPlatform.DatabaseInitialization/PaymentPlatform.DatabaseInitialization.csproj SupportApplications/PaymentPlatform.DatabaseInitialization/
COPY . .
WORKDIR /src/SupportApplications/PaymentPlatform.DatabaseInitialization

FROM build AS publish
RUN dotnet publish PaymentPlatform.DatabaseInitialization.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentPlatform.DatabaseInitialization.dll"]
