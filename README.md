# ChitChat_Server
 
## Docker

### Docker Hub

```
docker pull seeleo/chitchat:latest
sudo docker run --name chitchat -d -p 5001:80 --restart=always seeleo/chitchat:latest
```

### Container Registry (GitHub)

```
docker pull ghcr.io/lzcapp/chitchat:latest
sudo docker run --name chitchat -d -p 5001:80 --restart=always ghcr.io/lzcapp/chitchat:latest
```
