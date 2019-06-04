# Sql2Elastic

Sql2Elastic is an application that migrates your database tables to Elasticsearch documents.
This is made generic, so it doesn't matter how your table is structured.
Checkout this repository on [Github](https://github.com/Ryukote/Sql2Elastic).
The best scenario would be to migrate log tables to Elasticsearch with this.
For now it supports migration from **SQL Server** and **Postgres.**

In the future it will support other databases like Oracle.

## How to run this image

```bash
docker run --name sql2elastic --restart always -e "DbType=[type]" -e "SqlHost=[sql_address_with_port]" -e "DbPort=[db_port]" -e "DbName=[database_name]" -e "DbUsername=[database_username]" -e "DbPassword=[database_password]" -e "DbTable=[table_to_migrate]" -e "DbSchema=[schema_name]" -e "ElasticHost=[elastic_address_with_port]" -e "ElasticIndex=[index_name]" -e "ElasticDocument=[document_name]" -d ryukote/sql2elastic:latest
```

### Notice

1. Replace **"[type]"** with either "SQL Server" or "Postgres"
2. Replace **"[sql_address_with_port]"** with valid information in format:
    * **ip_address, port"** if you are using SQL Server (in this case you need to remove DbPort variable)
    * **"ip_address** if you are using Postgres (in this case you need to include your Postgres port in DbPort variable)
3. Replace **"[database_name]"** with valid database name
4. Replace **"[database_username]"** with valid database user that have rights to access this database
5. Replace **"[database_password]"** with valid password of user
6. Replace **"[table_to_migrate]"** with valid database table. **Don't** specify table schema in this parameter
7. Replace **"[schema_name]"** with valid schema name of mentioned database table
8. Replace **"[elastic_address_with_port]"** with Elasticsearch IP address(you need to add **http/https** prefix to Elasticsearch IP address) and port
9. Replace **"[index_name]"** with whatever name you want to use for your index, but it needs to be all lower case
10. Replace **"[document_name]"** with whatever name you want to use for your document, but it needs to be all lower case
