## Introduction

Backend credit system for cybertown

Used technology:
 - Docker
 - .NET 6.0
 - Postgree 14
 - Adminer

Used IDE: https://www.jetbrains.com/rider/ 

# How start app:
 - Copy .env.dist.env as .env
 - (linux only, maybe mac, for postgree data in dir) Change POSTGREE_RUN_USER to your user id and group id. 
 - Run `docker-compose up -d`
 - See swagger and migrations informations.

# How stop app:
- Run `docker-compose down`

# Swagger documentation:
 - Swagger documentation is available **only in developer** mode of application.
 - Run docker APP and go to address http://127.0.0.1:8080/swagger/index.html 
 - Address is for default port, if you change address in .env, go to address with your port.

# Migrations behaviors:
1) In PRODUCTION mode, migrations start automatically.
2) In DEVELOPMENT mode, migrations autostart dependent on `USE_MIGRATIONS_IN_DEV=true/false` in .env

# How setting project for developing in docker:
- https://www.jetbrains.com/help/rider/Docker.html#using-docker-compose

# How to dev without backend in docker?
- Using docker for dev is little slow :-(
- Create .env.dev with content "POSTGREE_HOST=127.0.0.1"
- Start only postgree docker "./startDevPostgree.sh" or "docker-compose -f docker-compose.devel.yml up -d db adminer"
- Start your APP in IDE (run/debug)

# FAQ:
- I need clear DB: docker-compose down **-v**
- I need rebuild docker images (after pull) `docker-compose up -d --build`
- I need run migrations manual `dotnet ef database update`
- I need run APP without auth: set development mode, `DISABLE_AUTHENTICATION=true`

# Seed accounts for testing:
 - admin@test.cz - Admin user (can anything)
 - member@test.cz - Basic member (Can login, but cannot call API)
 - worker@test.cz - Worker member (Can login, can call API but need login on place to get permissions of place)
 - sales@test.cz - Power Sales man (Can login, can call API but need login on place to get permissions of place + can create deposit transactions)

All users have: 
password: test
pin: 00000


# Ultimate solution if you have problem with deploy:
1) `docker-compose down -v`
2) Smaž složku `./Database/data`
3) V .env (přesná kopie .env.dist.env) změň enviroment na production viz comment.
4) `docker-compose up --build`
5) Počkej až se vše nastartuje a provedou se migrace.
6) Můžeš to zkontrolovat přes adminer.
7) `docker-compose down`
8) Změň .env na developer.
9) `docker-compose up --build`