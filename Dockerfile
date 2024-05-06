FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH

WORKDIR /bot
COPY src/*.csproj /bot/
RUN dotnet restore -a $TARGETARCH

FROM build as publish
WORKDIR /bot
COPY . .
RUN dotnet publish -a $TARGETARCH --no-restore src/AivaptDotNet.csproj -o src/bin/Release/net

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS runtime
WORKDIR /bot
COPY --from=publish /bot/src/bin/Release/net /bot/bin/Release/net
ENTRYPOINT ["./bin/Release/net/AivaptDotNet"]
