# 🐳 Gestión de Órdenes - Docker

Esta guía te ayudará a ejecutar la API de Gestión de Órdenes usando Docker.

## 📋 Prerrequisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y ejecutándose
- Al menos 4GB de RAM disponible
- Puertos 5111 y 1433 disponibles

## 🚀 Inicio Rápido

### Opción 1: Usar script automatizado (Windows)
```bash
# Ejecutar el script que construye e inicia todo
docker-run.bat
```

### Opción 2: Comandos manuales
```bash
# Construir e iniciar todos los servicios
docker-compose up --build -d

# Ver los logs
docker-compose logs -f

# Detener todos los servicios
docker-compose down
```

## 🏗️ Arquitectura Docker

La aplicación se compone de 2 contenedores:

### 1. **gestion-ordenes-api** (Puerto 5111)
- API .NET 8
- Swagger UI disponible
- Health check en `/health`

### 2. **gestion-ordenes-db** (Puerto 1433)
- SQL Server 2022 Express
- Base de datos persistente
- Inicialización automática

## 🌐 Acceso a la Aplicación

Una vez iniciada, puedes acceder a:

- **API Base**: http://localhost:5111
- **Swagger UI**: http://localhost:5111
- **Health Check**: http://localhost:5111/health

## 🗄️ Base de Datos

### Conexión desde aplicaciones externas:
- **Server**: `localhost,1433`
- **Database**: `GestionOrdenes`
- **Usuario**: `sa`
- **Contraseña**: `GestionOrdenes2024!`

### Conexión desde SQL Server Management Studio:
```
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: GestionOrdenes2024!
```

## 🔧 Comandos Útiles

### Gestión de contenedores
```bash
# Ver estado de los contenedores
docker-compose ps

# Ver logs en tiempo real
docker-compose logs -f api

# Reiniciar solo la API
docker-compose restart api

# Reconstruir imagen de la API
docker-compose up --build api

# Eliminar todo (incluyendo volúmenes)
docker-compose down -v
```

### Debugging
```bash
# Acceder al contenedor de la API
docker exec -it gestion-ordenes-api bash

# Acceder al contenedor de SQL Server
docker exec -it gestion-ordenes-db bash

# Ver logs de un servicio específico
docker-compose logs sqlserver
```

## 📊 Monitoreo

### Health Checks
- **API**: http://localhost:5111/health
- **Database**: Se verifica automáticamente con `sqlcmd`

### Logs
```bash
# Logs de todos los servicios
docker-compose logs -f

# Solo logs de la API
docker-compose logs -f api

# Solo logs de la base de datos
docker-compose logs -f sqlserver
```

## 🔒 Autenticación

La API usa JWT. Para autenticarte:

1. **POST** `/api/auth/login`
   ```json
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. Usar el token en el header: `Authorization: Bearer {token}`

## 🚨 Solución de Problemas

### Puerto ocupado
Si el puerto 5111 o 1433 está ocupado:
```bash
# Cambiar puertos en docker-compose.yml
ports:
  - "5112:8080"  # Cambiar 5111 por 5112
```

### Base de datos no conecta
```bash
# Verificar que SQL Server esté healthy
docker-compose ps

# Ver logs de SQL Server
docker-compose logs sqlserver

# Reiniciar base de datos
docker-compose restart sqlserver
```

### Reconstruir desde cero
```bash
# Eliminar todo y empezar de nuevo
docker-compose down -v
docker system prune -f
docker-compose up --build -d
```

## 📈 Rendimiento

### Recursos recomendados:
- **CPU**: 2 cores mínimo
- **RAM**: 4GB mínimo
- **Almacenamiento**: 2GB disponible

### Optimizaciones:
- Los datos de SQL Server se persisten en un volumen Docker
- La imagen de la API se optimiza con multi-stage build
- Health checks previenen tráfico a contenedores no listos

## 🔄 Actualizaciones

Para actualizar la aplicación:
```bash
# Reconstruir con cambios
docker-compose up --build -d

# O forzar reconstrucción completa
docker-compose build --no-cache api
docker-compose up -d
```

