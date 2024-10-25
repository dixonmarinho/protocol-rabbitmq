docker build -f Dockerfile.publish -t protocol-publish:latest .
docker run --name publish protocol-publish:latest
