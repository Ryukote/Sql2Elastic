# Elasticsearch database hook
Project that hooks on relational database and migrate data to Elasticsearch.
Application does not care about where database is comming from, what is the table name where you store logs, how many columns it have
and what data types are those columns of. Application will map data types from database to Elasticsearch and get all column names,
so Elasticsearch can store data that is 1 on 1 with data in SQL Server.

Application is hooking on database every 10 minute, to avoid potential handling of big data where it wouldn't have enough time to
do what it needs to to.

You can use this application with docker. Find more on: https://hub.docker.com/r/ryukote/elasticdatabasehook

# Types of databases that it can hook to
For now it can only hook to SQL Server

# You want to support this application?
If you think this application is great idea and you want to support this, you can help with issues, pull requests and small donations.
You can do donations with:
  - BTC on: 322SRqTS3EeKGaVFuo6xsw8e5Xji4QcJR6
  - ETH on: 0xc06d8766061e0644fb780f38abb1226ba289664c
  - XRP on: rE1sdh25BJQ3qFwngiTBwaq3zPGGYcrjp1 with destination tag: 59558
