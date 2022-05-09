# tcpserver-zadanie1-tc
## Jak zbuildować obraz Docker

Aby zbuildować obraz trzeba:
- sklonować repozytorium
- zbuildować obraz: `DOCKER_BUILDKIT=1 docker build -t local/zad1 .`

Aby urochomić kontener Docker-a: `docker run -d --name zad1 -p 8080:5000 local/zad1`
