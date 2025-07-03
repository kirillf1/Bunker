# Настройка базы данных в Bunker Helm Chart

Helm чарт поддерживает два режима работы с PostgreSQL:

## 1. Встроенная PostgreSQL (по умолчанию)

По умолчанию используется встроенная PostgreSQL через Bitnami chart:

```yaml
# values.yaml
postgresql:
  enabled: true          # Включить встроенную PostgreSQL
  
database:
  internal:
    port: 5432
    gameApiDatabase: bunker_game_db
    gameComponentsDatabase: bunker_game_components_db
    username: postgres
```

**Установка:**
```bash
helm install bunker ./deployment/bunker-helm
```

**Хост базы данных будет автоматически:** `{{ .Release.Name }}-postgresql`

## 2. Внешняя PostgreSQL

Для использования внешней PostgreSQL:

```yaml
# values-external-db.yaml
postgresql:
  enabled: false         # Отключить встроенную PostgreSQL        # Не устанавливать Bitnami chart

database:
  external:
    host: "your-external-postgres.example.com"
    port: 5432
    gameApiDatabase: bunker_game_db
    gameComponentsDatabase: bunker_game_components_db
    username: postgres

secrets:
  postgresql:
    create: true
    adminPassword: "YourExternalDbPassword"
    userPassword: "YourExternalDbUserPassword"
    replicationPassword: "YourExternalDbReplicationPassword"
```

**Установка:**
```bash
helm install bunker ./deployment/bunker-helm -f values-external-db.yaml
```

## 3. Переключение между режимами

### Переход с встроенной на внешнюю:

1. Создайте дамп данных из встроенной PostgreSQL:
```bash
kubectl exec -it deployment/bunker-release-postgresql -- pg_dump -U postgres bunker_game_db > backup.sql
```

2. Обновите чарт на внешнюю БД:
```bash
helm upgrade bunker ./deployment/bunker-helm -f values-external-db.yaml
```

### Переход с внешней на встроенную:

```bash
helm upgrade bunker ./deployment/bunker-helm -f values.yaml
```

## 4. Строки подключения

Чарт автоматически создает правильные строки подключения в секрете `postgresql-secret`:

### Для встроенной PostgreSQL:
```
Host={{ .Release.Name }}-postgresql;Port=5432;Database=bunker_game_db;Username=postgres;Password={{ password }}
```

### Для внешней PostgreSQL:
```
Host=your-external-postgres.example.com;Port=5432;Database=bunker_game_db;Username=postgres;Password={{ password }}
```

## 5. Проверка подключения

После установки проверьте секреты:

```bash
# Проверить секрет
kubectl get secret postgresql-secret -o yaml

# Декодировать строку подключения
kubectl get secret postgresql-secret -o jsonpath='{.data.bunker-game-api-postgres}' | base64 -d
```

## 6. Примеры использования

### AWS RDS PostgreSQL:
```yaml
database:
  external:
    host: "mydb.123456789012.us-east-1.rds.amazonaws.com"
    port: 5432
    gameApiDatabase: bunker_game_db
    gameComponentsDatabase: bunker_game_components_db
    username: postgres

secrets:
  postgresql:
    adminPassword: "MyRDSPassword"
```

### Azure Database for PostgreSQL:
```yaml
database:
  external:
    host: "myserver.postgres.database.azure.com"
    port: 5432
    gameApiDatabase: bunker_game_db
    gameComponentsDatabase: bunker_game_components_db
    username: "myuser@myserver"

secrets:
  postgresql:
    adminPassword: "MyAzurePassword"
```

### Google Cloud SQL:
```yaml
database:
  external:
    host: "127.0.0.1"  # Через Cloud SQL Proxy
    port: 5432
    gameApiDatabase: bunker_game_db
    gameComponentsDatabase: bunker_game_components_db
    username: postgres

secrets:
  postgresql:
    adminPassword: "MyCloudSQLPassword"
```

## 7. Безопасность

⚠️ **Важные моменты безопасности:**

1. **Никогда не коммитьте пароли** в values файлы в Git
2. **Используйте внешние секреты** для продакшена:
   ```yaml
   secrets:
     postgresql:
       create: false
       existingSecret: "my-external-secret"
   ```
3. **Включите TLS** для внешних подключений
4. **Используйте сильные пароли** для продакшена

## 8. Миграции

При смене типа базы данных необходимо:

1. Остановить приложения
2. Создать backup
3. Настроить новую БД
4. Восстановить данные
5. Обновить Helm релиз 

# Использование внешних секретов

## Логика работы секретов

Helm chart поддерживает упрощенную логику управления секретами:

### 1. Простая логика deployment:
- **Если указан `existingSecret` у сервиса** → используем готовые connection strings из него
- **Если не указан `existingSecret` у сервиса** → используем автосозданный секрет

### 2. Логика создания автосекретов:
Автосекреты создаются только если:
- `gameApi.secrets.existingSecret = ""`
- И `postgresql.secrets.existingSecret = ""`

В автосекретах connection strings собираются из:
- **Host/Port/Database** → из values.yaml (internal/external настройки)
- **Username/Password** → из `postgresql.secrets.adminPassword`

### 3. Структура секретов:

#### Индивидуальный секрет сервиса (existingSecret):
```yaml
data:
  postgres-connection: "Host=...;Database=...;Username=...;Password=..."
  postgres-readonly-connection: "Host=...;Database=...;Username=...;Password=..."
```

#### Автосозданный секрет:
```yaml
data:
  postgres-connection: "Host=primary;Port=5432;Database=gameapi;Username=postgres;Password=postgres"
  postgres-readonly-connection: "Host=read;Port=5432;Database=gameapi;Username=postgres;Password=postgres"
```

### 4. Сценарии использования:

#### A. Индивидуальные секреты (рекомендуется):
```yaml
gameApi:
  secrets:
    existingSecret: "game-api-secret"
gameComponentsApi:
  secrets:
    existingSecret: "components-api-secret"
postgresql:
  secrets:
    existingSecret: "postgres-secret"  # для самого PostgreSQL
```

#### B. Автосоздание (для разработки):
```yaml
gameApi:
  secrets:
    existingSecret: ""
gameComponentsApi:
  secrets:
    existingSecret: ""
postgresql:
  secrets:
    existingSecret: ""
    adminPassword: "strongpassword"
```

#### C. Смешанный режим:
```yaml
gameApi:
  secrets:
    existingSecret: "game-api-secret"
gameComponentsApi:
  secrets:
    existingSecret: ""  # автосекрет создастся
postgresql:
  secrets:
    existingSecret: ""
```

### 5. Deployment логика:

Каждый сервис использует простую логику:
```yaml
env:
- name: ConnectionStrings__PostgresConnection
  valueFrom:
    secretKeyRef:
      name: {{ if .Values.gameApi.secrets.existingSecret }}
             {{ .Values.gameApi.secrets.existingSecret }}
           {{ else }}
             {{ include "bunker-helm.fullname" . }}-game-api-secret
           {{ end }}
      key: {{ .Values.gameApi.secrets.connectionStringKey }}
```

### 6. Преимущества новой архитектуры:

✅ **Простота** - только 2 варианта в deployment  
✅ **Готовые connection strings** - в секретах, не в deployment  
✅ **Безопасность** - пароли только в секретах  
✅ **Гибкость** - можно использовать индивидуальные или общие секреты  
✅ **Избежание дублирования** - логика сборки только в secret template