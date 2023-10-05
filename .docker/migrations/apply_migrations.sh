#!/bin/bash

echo "Waiting for PostgreSQL to be ready..."

while ! pg_isready; do
  echo "Connecting to $PGHOST port $PGPORT with user $PGUSER..."
  sleep 0.5
done
echo "PostgreSQL connection ready."

# Create DB if not exists
psql -tc "SELECT 1 FROM pg_database WHERE datname = '$DATABASE_NAME'" | grep -q 1 || psql -c "CREATE DATABASE \"$DATABASE_NAME\""

# Apply script
export PGDATABASE=$DATABASE_NAME
psql -a -f $SQL_SCRIPT_NAME