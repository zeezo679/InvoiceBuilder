FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/InvoiceBuilder.Application.UnitTests/InvoiceBuilder.Application.UnitTests.csproj", "tests/InvoiceBuilder.Application.UnitTests/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
RUN dotnet restore "tests/InvoiceBuilder.Application.UnitTests/InvoiceBuilder.Application.UnitTests.csproj"
COPY . .
WORKDIR "/src/tests/InvoiceBuilder.Application.UnitTests"
RUN dotnet build "./InvoiceBuilder.Application.UnitTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InvoiceBuilder.Application.UnitTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoiceBuilder.Application.UnitTests.dll"]
