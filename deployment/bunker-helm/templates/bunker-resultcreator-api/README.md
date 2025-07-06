# Bunker ResultCreator API

## Описание
Сервис для создания результатов игры с использованием AI. Поддерживает работу с провайдерами Ollama и GigaChat.

## Конфигурация

### Включение сервиса
```yaml
resultCreatorApi:
  enabled: true
```

### Настройка чувствительных данных

Каждый блок конфигурации может использовать свой собственный секрет:

#### Вариант 1: Использование внешних секретов
```yaml
resultCreatorApi:
  app:
    kafka:
      existingSecret: "my-kafka-secret"
      secretKeys:
        loginKey: "kafka-login"
        passwordKey: "kafka-password"
    gigaChat:
      existingSecret: "my-gigachat-secret"
      secretKeys:
        secretKey: "gigachat-secret"
        clientIdKey: "gigachat-client-id"
        clientSecretKey: "gigachat-client-secret"
```

#### Вариант 2: Указание значений в values.yaml
```yaml
resultCreatorApi:
  app:
    kafka:
      existingSecret: ""  # Оставить пустым для автосоздания
      login: "my-kafka-login"
      password: "my-kafka-password"
    gigaChat:
      existingSecret: ""  # Оставить пустым для автосоздания
      secretKey: "my-gigachat-secret-key"
      clientId: "my-gigachat-client-id"
      clientSecret: "my-gigachat-client-secret"
```

### Настройка AI провайдера

#### Ollama
```yaml
resultCreatorApi:
  app:
    ai:
      provider: "Ollama"
    ollama:
      baseUrl: "http://ollama-service:11434"
      model: "owl/t-lite"
```

#### GigaChat
```yaml
resultCreatorApi:
  app:
    ai:
      provider: "GigaChat"
    gigaChat:
      secretKey: "your-gigachat-secret"
      isCommercial: true
      defaultModel: "GigaChat-2"
```

### Настройка Kafka
```yaml
resultCreatorApi:
  app:
    kafka:
      servers: "kafka-cluster:9092"
      login: "kafka-user"
      password: "kafka-password"
      createGameResultRequestsTopicName: "game-result-requests"
      createGameResultResponsesTopicName: "game-result-responses"
```

## Пример полной конфигурации

```yaml
resultCreatorApi:
  enabled: true
  replicaCount: 2
  
  app:
    ai:
      provider: "Ollama"
      parallelAgentWorkers: 4
      chat:
        temperature: 0.8
        maxOutputTokens: 2048
        
    ollama:
      baseUrl: "http://ollama-service:11434"
      model: "llama3"
      
    kafka:
      existingSecret: ""
      servers: "kafka:9092"
      login: ""
      password: ""
      
    gigaChat:
      existingSecret: ""
      secretKey: ""
      clientId: ""
      clientSecret: ""
      
  resources:
    requests:
      memory: "1Gi"
      cpu: "500m"
    limits:
      memory: "2Gi"
      cpu: "1000m"
```

## Важные замечания

1. **Только нужные секреты**: Секреты создаются только для действительно чувствительных данных (пароли, API ключи)
2. **Чувствительные данные**: Kafka логин/пароль и GigaChat ключи всегда сохраняются в секретах Kubernetes
3. **Провайдер AI**: Выберите один из провайдеров - Ollama или GigaChat
4. **Ollama**: Обычно не требует аутентификации, поэтому секреты не нужны
5. **Ресурсы**: Для AI задач рекомендуется увеличить память до 1-2 GB
6. **Масштабирование**: Можно увеличить `parallelAgentWorkers` и `replicaCount` для повышения производительности

## Добавление новых чувствительных данных

Если вам нужно добавить новые чувствительные данные, следуйте этому шаблону:

1. **В values.yaml** добавьте блок с настройками секрета:
```yaml
resultCreatorApi:
  app:
    myNewService:
      # Настройки секрета для нового сервиса
      existingSecret: ""
      secretKeys:
        apiKeyKey: "my-service-api-key"
        passwordKey: "my-service-password"
      
      # Чувствительные данные (используются если existingSecret пуст)
      apiKey: ""
      password: ""
      # Обычные настройки
      baseUrl: "https://my-service.com"
```

2. **В secret.yaml** добавьте создание секрета:
```yaml
{{- if not .Values.resultCreatorApi.app.myNewService.existingSecret }}
# MyNewService секрет
apiVersion: v1
kind: Secret
data:
  my-service-api-key: {{ .Values.resultCreatorApi.app.myNewService.apiKey | b64enc }}
  my-service-password: {{ .Values.resultCreatorApi.app.myNewService.password | b64enc }}
{{- end }}
```

3. **В deployment.yaml** добавьте переменные окружения:
```yaml
- name: MyNewService__ApiKey
  valueFrom:
    secretKeyRef:
      name: {{ if .Values.resultCreatorApi.app.myNewService.existingSecret }}...{{ end }}
      key: {{ if .Values.resultCreatorApi.app.myNewService.existingSecret }}...{{ end }}
```

## Созданные секреты

При автоматическом создании секретов создаются следующие секреты:
- `{release-name}-resultcreator-api-kafka-secret` - для чувствительных данных Kafka (login, password)
- `{release-name}-resultcreator-api-gigachat-secret` - для чувствительных данных GigaChat (secretKey, clientId, clientSecret) 