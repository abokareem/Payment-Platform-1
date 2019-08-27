FROM mcr.microsoft.com/dotnet/core/runtime:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY SupportApplications/DatabaseInitialization/PaymentPlatform.Initialization.UI.ConsoleApp/PaymentPlatform.Initialization.UI.ConsoleApp.csproj SupportApplications/DatabaseInitialization/PaymentPlatform.Initialization.UI.ConsoleApp/
COPY . .
WORKDIR /src/SupportApplications/DatabaseInitialization/PaymentPlatform.Initialization.UI.ConsoleApp

FROM build AS publish
RUN dotnet publish PaymentPlatform.Initialization.UI.ConsoleApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentPlatform.Initialization.UI.ConsoleApp.dll"]
