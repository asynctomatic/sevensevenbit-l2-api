# Makefile
# Main Makefile that includes all other Makefile components.

# Include split parts
include .make/docker.mk
include .make/tests.mk
include .make/db_migrations.mk

# The primary target to build and run the application.
all: build run