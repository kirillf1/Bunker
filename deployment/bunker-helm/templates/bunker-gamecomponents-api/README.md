# Bunker GameComponents API

## Описание
API для управления игровыми компонентами (карты персонажей, катастрофы, бункеры и т.д.).

## Конфигурация

### Включение сервиса
```yaml
gameComponentsApi:
  enabled: true
```

### Настройка чувствительных данных

#### PostgreSQL
```yaml
gameComponentsApi:
  app:
    postgres:
      existingSecret: ""  # Оставить пустым для автосоздания
      secretKeys:
        connectionStringKey: "postgres-connection"
        readReplicaConnectionKey: "postgres-readonly-connection"
```

**Вариант с внешним секретом:**
```yaml
gameComponentsApi:
  app:
    postgres:
      existingSecret: "my-postgres-secret"
      secretKeys:
        connectionStringKey: "postgres-connection"
        readReplicaConnectionKey: "postgres-readonly-connection"
```

### Настройка логирования
```yaml
gameComponentsApi:
  app:
    logging:
      level: Information
      microsoftLevel: Warning
      aspnetCoreLevel: Error
      hostingLevel: Information
```

### Настройка OpenTelemetry
```yaml
gameComponentsApi:
  app:
    telemetry:
      enabled: true
      namespace: monitoring
      collectorService: otel-collector-opentelemetry-collector
      collectorPort: 4317
```

## Пример полной конфигурации

```yaml
gameComponentsApi:
  enabled: true
  replicaCount: 1
  
  app:
    postgres:
      existingSecret: ""  # Автосоздание секрета для PostgreSQL
      
    environment: Production
    
    logging:
      level: Information
      microsoftLevel: Warning
      
    telemetry:
      enabled: true
      namespace: monitoring
      
  resources:
    requests:
      memory: "256Mi"
      cpu: "250m"
    limits:
      memory: "512Mi"
      cpu: "500m"
      
  healthChecks:
    startup:
      enabled: true
    liveness:
      enabled: true
    readiness:
      enabled: true
```

## Созданные секреты

При автоматическом создании секретов создаются следующие секреты:
- `{release-name}-gamecomponents-api-secret` - для PostgreSQL подключений

## Важные замечания

1. **PostgreSQL**: Автоматически настраивается подключение к внутренней или внешней БД
2. **Простота**: Этот сервис содержит только базовые настройки и PostgreSQL
3. **Зависимости**: Game API использует этот сервис для получения игровых компонентов
4. **Масштабирование**: Обычно достаточно одного экземпляра, но можно увеличить `replicaCount`

## Добавление чувствительных данных

Если в будущем потребуется добавить чувствительные данные (например, внешние API ключи), следуйте шаблону:

```yaml
gameComponentsApi:
  app:
    externalService:
      # Настройки секрета для внешнего сервиса
      existingSecret: ""
      secretKeys:
        apiKeyKey: "external-api-key"
      
      # Чувствительные данные (используются если existingSecret пуст)
      apiKey: ""
      # Обычные настройки
      baseUrl: "https://external-service.com"
```

Затем создайте соответствующий секрет и добавьте переменные окружения в deployment. 