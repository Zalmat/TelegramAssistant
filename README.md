**TelegramAssistant**

Телеграм бот для расчёта SIGN CKASSA
***
**Запуск через командную строку**
```
$ docker run -d \
-e BOTTOKEN=MySecretTelegramToken \
-v /home/user/logs:/app/logs \
zalmat/telegrambotsigner
```

**Запуск через Docker-compose**

```
version: '3.4'

services:
  telegrambotsigner:
    container_name: telegrambotsigner
    image: telegrambotsigner
    restart: unless-stopped
    environment:
      - BOTTOKEN=MySecretTelegramToken
    volumes:
      - /home/user/logs:/app/logs
```



Docker: https://hub.docker.com/r/zalmat/telegrambotsigner

