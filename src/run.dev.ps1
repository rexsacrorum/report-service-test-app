# I Use old docker-compose, not docker compose subcommand, because it's supports --env-file parametr.
docker-compose `
    -f docker-compose.yml -f docker-compose.override.yml `
    --env-file .env `
    -p "report-service" `
    --profile db `
    --profile backend `
    up `
# up --build reportservice `
