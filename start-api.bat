docker build -f Dockerfile.api -t protocol-api:latest .
docker run --name api protocol-api:latest