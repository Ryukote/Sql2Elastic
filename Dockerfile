FROM microsoft/dotnet:2.1-runtime AS base

#arguments
ARG HookInterval=foo
ARG SqlHost=foo
ARG DbName=foo
ARG DbUsername=foo
ARG DbPassword=foo
ARG DbTable=foo
ARG ElasticHost=foo
ARG ElasticIndex=foo
ARG ElasticDocument=foo
ARG ShardNum=1

#how often will app be hooked to sql server in miliseconds
ENV HookInterval ${HookInterval}
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
#host where Elasticsearch is located
ENV ElasticHost $ElasticHost
#index name
ENV ElasticIndex ${ElasticIndex}
#index document name
ENV ElasticDocument ${ElasticDocument}
#number of shards
ENV ShardNum ${ShardNum}

WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ElasticSQLServer/ElasticSQLServer.csproj ElasticSQLServer/
RUN dotnet restore ElasticSQLServer/ElasticSQLServer.csproj
COPY . .
WORKDIR /src/ElasticSQLServer
RUN dotnet build ElasticSQLServer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ElasticSQLServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ElasticSQLServer.dll"]
