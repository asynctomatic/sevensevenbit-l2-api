# docker.mk
# Contains rules related to Docker and Docker Compose operations.
.PHONY: *

# Set environment to dev by default
ENV ?= dev

# Docker compose command and file specification
ifeq ($(ENV),prod)
	DC := docker compose -f docker-compose.yml
else
	DC := docker compose -f docker-compose.dev.yml
endif

# Builds and runs the worker service using Docker Compose.
worker:
	@$(DC) build worker
	@$(DC) up -d worker

# Builds and runs the database services using Docker Compose.
database:
	@$(DC) build db
	@$(DC) up -d db db-admin 

# Builds and runs the starkex service using Docker Compose.
starkex:
	@$(DC) build starkex
	@$(DC) up -d starkex

# Builds the main application components using Docker Compose.
build: build-infra
	@$(DC) build api worker

# Builds infrastructure components using Docker Compose.
build-infra:
	@$(DC) build db db-operator-migrations db-blockchain-migrations db-admin message-bus starkex

# Runs the main application components using Docker Compose.
run: run-infra
	@$(DC) up -d api worker

# Runs infrastructure components using Docker Compose.
run-infra:
	@$(DC) up -d db db-operator-migrations db-blockchain-migrations db-admin message-bus starkex

# Stops all services managed by Docker Compose.
stop:
	@$(DC) down
