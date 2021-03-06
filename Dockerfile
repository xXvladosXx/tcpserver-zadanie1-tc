FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./tcpnew/tcpnew.csproj"
RUN dotnet publish "./tcpnew/tcpnew.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "tcpnew.dll"]
