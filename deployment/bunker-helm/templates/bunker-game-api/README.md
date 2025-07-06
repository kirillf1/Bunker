# Bunker Game API

## Описание
Основной игровой API, обрабатывающий игровую логику и взаимодействие с другими сервисами.

## Конфигурация

### Включение сервиса
```yaml
gameApi:
  enabled: true
```

### Настройка чувствительных данных

#### PostgreSQL
```yaml
gameApi:
  app:
    postgres:
      existingSecret: ""  # Оставить пустым для автосоздания
      secretKeys:
        connectionStringKey: "postgres-connection"
        readReplicaConnectionKey: "postgres-readonly-connection"
```

#### Kafka настройки

**Вариант 1: Использование внешнего секрета**
```yaml
gameApi:
  app:
    kafka:
      existingSecret: "my-kafka-secret"
      secretKeys:
        loginKey: "kafka-login"
        passwordKey: "kafka-password"
```

**Вариант 2: Указание значений в values.yaml**
```yaml
gameApi:
  app:
    kafka:
      existingSecret: ""  # Оставить пустым для автосоздания
      login: "my-kafka-login"
      password: "my-kafka-password"
```

### Настройка сервисов

#### GameComponents API
```yaml
gameApi:
  app:
    gameComponents:
      address: "http://gamecomponents-service:80"  # Переопределить адрес при необходимости
```

#### Kafka
```yaml
gameApi:
  app:
    kafka:
      servers: "kafka-cluster:9092"
      createGameResultRequestsTopicName: "game-result-requests"
      createGameResultResponsesTopicName: "game-result-responses"
```

## Пример полной конфигурации

```yaml
gameApi:
  enabled: true
  replicaCount: 2
  
  app:
    postgres:
      existingSecret: ""
      
    gameComponents:
      address: ""  # Использовать автоматический адрес
      
    kafka:
      existingSecret: ""
      servers: "kafka:9092"
      login: ""
      password: ""
      
  resources:
    requests:
      memory: "512Mi"
      cpu: "250m"
    limits:
      memory: "1Gi"
      cpu: "500m"
```

## Созданные секреты

При автоматическом создании секретов создаются следующие секреты:
- `{release-name}-game-api-secret` - для PostgreSQL подключений
- `{release-name}-game-api-kafka-secret` - для чувствительных данных Kafka (login, password)

## Важные замечания

1. **PostgreSQL**: Автоматически настраивается подключение к внутренней или внешней БД
2. **Kafka**: Используется для отправки запросов на создание результатов игры в ResultCreator API
3. **GameComponents**: Автоматически подключается к GameComponents API в том же namespace
4. **Масштабирование**: Можно увеличить `replicaCount` для высокой доступности 