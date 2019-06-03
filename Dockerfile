FROM microsoft/dotnet:2.1-runtime AS base

#arguments
ARG SqlHost=foo
ARG DbName=foo
ARG DbUsername=foo
ARG DbPassword=foo
ARG DbTable=foo
ARG DbSchema=foo
ARG ElasticHost=foo
ARG ElasticIndex=foo
ARG ElasticDocument=foo

#host where database is located
ENV SqlHost ${SqlHost}
#database name
ENV DbName ${DbName}
#database user
ENV DbUsername ${DbUsername}
#database user password
ENV DbPassword ${DbPassword}
#table to migrate to Elasticsearch
ENV DbTable ${DbTable}
#table schema
ENV DbSchema ${DbSchema}
#host where Elasticsearch is located
ENV ElasticHost $ElasticHost
#index name
ENV ElasticIndex ${ElasticIndex}
#index document name
ENV ElasticDocument ${ElasticDocument}

WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Sql2Elastic/Sql2Elastic.csproj Sql2Elastic/
RUN dotnet restore Sql2Elastic/Sql2Elastic.csproj
COPY . .
WORKDIR /src/Sql2Elastic
RUN dotnet build Sql2Elastic.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Sql2Elastic.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Sql2Elastic.dll"]
