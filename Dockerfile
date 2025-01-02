FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

ARG TARGETARCH
RUN dotnet publish src/Serein.Cli -a $TARGETARCH -c Release -o /app/bin

WORKDIR /app/bin
ENV DOTNET_EnableWriteXorExecute=0

RUN echo "#!/bin/sh \r\n/app/bin/Serein.Cli" > /app/bin/serein-entrypoint.sh
RUN chmod +x /app/bin/serein-entrypoint.sh

ENTRYPOINT [ "/app/bin/serein-entrypoint.sh" ]