FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS publish
WORKDIR /src

COPY ["Terminator.Web/Terminator.Web.csproj", "Terminator.Web/"]
COPY ["Terminator.Infrastructure/Terminator.Infrastructure.csproj", "Terminator.Infrastructure/"]
COPY ["Terminator.Application/Terminator.Application.csproj", "Terminator.Application/"]
COPY ["Terminator.Core/Terminator.Core.csproj", "Terminator.Core/"]

RUN dotnet restore "Terminator.Web/Terminator.Web.csproj" --runtime linux-musl-x64

COPY . .

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Terminator.Web/Terminator.Web.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/publish  \
    -p:UseAppHost=false \
    --runtime linux-musl-x64 \
    --no-restore
    # UseAppHost=false - we don't need a native executable

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
EXPOSE 8080
RUN mkdir /app/data && chown $APP_UID /app/data
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Terminator.Web.dll"]
