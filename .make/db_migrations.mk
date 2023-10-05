# db_migrations.mk
# Contains rules for handling database migrations.

# Migration paths and context
MIGRATIONS_PROJECT := ./src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj
MIGRATIONS_STARTUP_PROJECT := ./src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj
context := Operator

.PHONY: migrations-script
# Generates an idempotent migrations script for the current database state.
migrations-script:
	@dotnet ef migrations script \
    		--idempotent \
    		--project $(MIGRATIONS_PROJECT) \
    		--startup-project $(MIGRATIONS_STARTUP_PROJECT) \
    		--output ./src/SevenSevenBit.Operator.Infrastructure.SQL/Data/$(context)Data/Migrations/SQL/Migrations.sql \
    		--context $(context)DbContext

.PHONY: migrations-add
# Adds a new migration with the provided name.
migrations-add:
	@dotnet ef migrations add $(name) \
    		--project $(MIGRATIONS_PROJECT) \
    		--startup-project $(MIGRATIONS_STARTUP_PROJECT) \
    		--output-dir ./Data/$(context)Data/Migrations/Code \
    		--context $(context)DbContext

.PHONY: migrations-list
# Lists all available migrations.
migrations-list:
	@dotnet ef migrations list \
    		--project $(MIGRATIONS_PROJECT) \
    		--startup-project $(MIGRATIONS_STARTUP_PROJECT) \
    		--context $(context)DbContext

.PHONY: migrations-update
# Updates the database to the specified migration or the latest available.
migrations-update:
	@ASPNETCORE_ENVIRONMENT=Development dotnet ef database update $(name) \
        	--project $(MIGRATIONS_PROJECT) \
        	--startup-project $(MIGRATIONS_STARTUP_PROJECT) \
        	--context $(context)DbContext

.PHONY: migrations-remove
# Removes the most recent migration.
migrations-remove:
	@ASPNETCORE_ENVIRONMENT=Development dotnet ef migrations remove \
    		--project $(MIGRATIONS_PROJECT) \
    		--startup-project $(MIGRATIONS_STARTUP_PROJECT) \
    		--context $(context)DbContext