FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /bot
COPY src/*.csproj /bot/
RUN dotnet restore

FROM build as publish
WORKDIR /bot
COPY . .
RUN dotnet publish src/AivaptDotNet.csproj -c Release -o src/bin/Release/net

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /bot
COPY --from=publish /bot/src/bin/Release/net /bot/bin/Release/net
ENTRYPOINT ["./bin/Release/net/AivaptDotNet"]
