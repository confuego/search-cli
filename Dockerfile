FROM ubuntu as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /src
COPY . .
RUN dotnet publish ./Search.Cli -c Release -o /app -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
RUN mv /app/Search.Cli /app/search
RUN rm /app/Search.Cli.pdb

FROM base as final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
COPY ./data /app/data
COPY --from=build /app /usr/bin