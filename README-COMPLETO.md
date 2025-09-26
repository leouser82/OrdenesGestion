# 📈 Gestión de Órdenes API

(PARARSE EN LA RAMA MASTER EN GIT)

API REST completa para la gestión de órdenes de inversión en activos financieros, desarrollada con .NET 8 siguiendo principios SOLID y arquitectura limpia.

## 🚀 Características Principales

### ✨ Funcionalidades Core
- **Gestión de Órdenes de Inversión**: Crear, consultar, ejecutar y cancelar órdenes
- **Estados de Orden**: Seguimiento completo del ciclo de vida (En proceso, Ejecutada, Cancelada)
- **Cálculos Automáticos**: Cálculo automático de comisiones, impuestos y montos totales
- **Datos de Referencia**: Gestión centralizada de tipos de activo y estados

### 🔒 Seguridad
- **Autenticación JWT**: Tokens seguros con expiración configurable
- **Validación de Modelos**: Validaciones robustas en DTOs
- **Middleware de Excepciones**: Manejo centralizado y seguro de errores

### 🏗️ Arquitectura
- **Principios SOLID**: Código mantenible y extensible
- **Repository Pattern**: Abstracción de acceso a datos
- **Dependency Injection**: Inversión de dependencias nativa de .NET
- **Entity Framework Core**: ORM moderno con SQL Server
- **Mapeo Manual**: Control total sobre transformaciones de datos

### 🐳 Despliegue
- **Docker Ready**: Configuración completa con docker-compose
- **Health Checks**: Monitoreo de estado de aplicación y base de datos
- **Scripts Automatizados**: Inicio rápido con scripts de Windows
- **Múltiples Ambientes**: Configuración para Development y Production

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET | 8.0 | Framework principal |
| Entity Framework Core | 8.0 | ORM y acceso a datos |
| SQL Server | 2022 Express | Base de datos |
| JWT Bearer | 8.0 | Autenticación |
| Swagger/OpenAPI | 6.4.0 | Documentación API |
| Docker | Latest | Containerización |

## 📊 Modelo de Datos

### Entidades Principales

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   TipoActivo    │    │ ActivoFinanciero │    │  OrdenInversion │
├─────────────────┤    ├──────────────────┤    ├─────────────────┤
│ Id              │◄──┤│ Id               │◄──┤│ Id              │
│ Nombre          │    │ Nombre           │    │ CuentaId        │
│ Descripcion     │    │ Ticker           │    │ ActivoId        │
│ Comision        │    │ Precio           │    │ Cantidad        │
│ Impuesto        │    │ TipoActivoId     │    │          		 │
└─────────────────┘    │ FechaActualiz... │    │ Operacion       │
                       └──────────────────┘    │ EstadoId        │
┌─────────────────┐                            │ MontoTotal      │
│   EstadoOrden   │                            │ Comision        │
├─────────────────┤                            │ Impuesto        │
│ Id              │◄──────────────────────────┤└─────────────────┘
│ Nombre          │
│ Descripcion     │
└─────────────────┘
```

### Datos de Referencia Pre-cargados en la base de datos

**Estados de Orden:**
1. En proceso
2. Ejecutada
3. Cancelada

**Tipos de Activo:**
1. Acción (Comisión: 0.6%, Impuesto: 21%)
2. Bono (Comisión: 0.2%, Impuesto: 21%)
3. FCI (Sin comisión ni impuesto)

**Activos Financieros:**
- **Acciones**: AAPL, GOOGL, MSFT, KO, WMT
- **Bonos**: AL30, GD30
- **FCI**: Delta Pesos, Fima Premium

## 🔧 Instalación y Configuración

### Prerrequisitos

#### Para ejecución local:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (incluido con Visual Studio)

#### Para ejecución con Docker:
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Al menos 4GB de RAM disponible
- Puertos 5111 y 1433 disponibles

### 🚀 Opción 1: Ejecución Local

1. **Clonar el repositorio**
   ```bash
   git clone <repository-url>
   cd GestionOrdenes
   ```

2. **Restaurar paquetes NuGet**
   ```bash
   dotnet restore
   ```

3. **Configurar la base de datos**
   
   La aplicación usa SQL Server LocalDB por defecto. La cadena de conexión está en `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GestionOrdenes;Integrated Security=True;..."
     }
   }
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a la API**
   - La aplicación se ejecutará en `https://localhost:7xxx` (puerto mostrado en consola)
   - Swagger UI disponible en la URL raíz
   - Health check en `/health`

### 🐳 Opción 2: Ejecución con Docker

#### Inicio Rápido (Windows)
```bash
# Ejecutar script automatizado
docker-run.bat
```

#### Comandos Manuales
```bash
# Construir e iniciar todos los servicios
docker-compose up --build -d

# Ver logs en tiempo real
docker-compose logs -f

# Detener servicios
docker-compose down
```

#### Acceso con Docker
- **API**: http://localhost:5111
- **Swagger UI**: http://localhost:5111
- **Health Check**: http://localhost:5111/health
- **Base de Datos**: localhost,1433 (sa/GestionOrdenes2024!)

## 🔐 Autenticación y Autorización

### Usuarios de Prueba

| Usuario | Contraseña | Rol | Permisos |
|---------|------------|-----|----------|
| `admin` | `admin123` | Admin | Todos los endpoints + cancelar órdenes |
| `user` | `user123` | User | Consultas y creación de órdenes |

### Proceso de Autenticación

1. **Obtener Token**
   ```http
   POST /api/auth/login
   Content-Type: application/json
   
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Respuesta**
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
     "expiration": "2024-12-20T10:30:00Z",
     "username": "admin",
     "role": "Admin"
   }
   ```

3. **Usar Token**
   ```http
   Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9... (dentro del boton Authorize)
   ```

## 📚 API Endpoints

### 🔑 Autenticación
| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| POST | `/api/auth/login` | Autenticar usuario | No |

### 📋 Datos de Referencia
| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/api/referencedata/estados-orden` | Obtener estados de orden | Sí |
| GET | `/api/referencedata/tipos-activo` | Obtener tipos de activo | Sí |
| GET | `/api/referencedata/activos-financieros` | Obtener activos financieros | Sí |

### 💰 Órdenes de Inversión
| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/ordenesinversion` | Obtener todas las órdenes | User/Admin |
| GET | `/api/ordenesinversion/{id}` | Obtener orden por ID | User/Admin |
| POST | `/api/ordenesinversion` | Crear nueva orden | User/Admin |
| PUT | `/api/ordenesinversion/{id}/ejecutar` | Ejecutar orden | User/Admin |
| DELETE | `/api/ordenesinversion/{id}/cancelar` | Cancelar orden | Solo Admin |
| GET | `/api/ordenesinversion/test-exception/{tipo}` | Test de excepciones | User/Admin |

### 🏥 Salud del Sistema
| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/health` | Estado de la aplicación | No |

## 💡 Ejemplos de Uso

### Flujo Completo de Creación de Orden

1. **Autenticarse**
   ```http
   POST /api/auth/login
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Obtener activos disponibles**
   ```http
   GET /api/referencedata/activos-financieros
   Authorization: Bearer {token}
   ```

3. **Crear orden de compra**
   ```http
   POST /api/ordenesinversion
   Authorization: Bearer {token}
   {
     "cuentaId": 12345,
     "activoId": 1,
     "cantidad": 10,
     "operacion": "C"
   }
   ```

4. **Ejecutar la orden**
   ```http
   PUT /api/ordenesinversion/1/ejecutar
   Authorization: Bearer {token}
   ```

### Ejemplos de Respuestas

**Crear Orden - Respuesta:**
```json
{
  "id": 1,
  "cuentaId": 12345,
  "activoId": 1,
  "cantidad": 10,
  "operacion": "C",
  "estadoId": 1,
  "montoTotal": 1779.70,
  "comision": 10.68,
  "impuesto": 373.74,
  "activoNombre": "Apple Inc.",
  "activoTicker": "AAPL",
  "estadoDescripcion": "En proceso"
}
```

## 🔧 Configuración Avanzada

### Variables de Entorno (Docker)

```yaml
# docker-compose.yml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ASPNETCORE_URLS=http://+:8080
  - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=GestionOrdenes;...
```

### Configuración JWT

```json
{
  "JwtSettings": {
    "SecretKey": "MySecretKey12345678901234567890123456789012345678901234567890",
    "Issuer": "GestionOrdenesAPI",
    "Audience": "GestionOrdenesAPI",
    "ExpirationInHours": 24
  }
}
```

### Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testing y Debugging

### Endpoint de Prueba de Excepciones

La API incluye un endpoint especial para probar el manejo de excepciones:

```http
GET /api/ordenesinversion/test-exception/{tipo}
```

Tipos disponibles:
- `1`: ArgumentNullException
- `2`: ArgumentException  
- `3`: InvalidOperationException
- `4`: UnauthorizedAccessException
- `5`: KeyNotFoundException
- `6`: Exception genérica

### Logs y Monitoreo

```bash
# Ver logs en Docker
docker-compose logs -f api

# Ver logs de base de datos
docker-compose logs -f sqlserver

# Estado de contenedores
docker-compose ps
```


#### Token JWT Expirado
- Los tokens expiran en 24 horas
- Realizar nuevo login para obtener token fresco


## 🔄 Desarrollo y Contribución

### Estructura del Proyecto

```
GestionOrdenes/
├── Controllers/           # Controladores REST
│   ├── AuthController.cs
│   ├── OrdenesInversionController.cs
│   └── ReferenceDataController.cs
├── Data/                 # DbContext y configuración EF
│   └── GestionOrdenesDbContext.cs
├── DTOs/                 # Data Transfer Objects
│   ├── AuthDto.cs
│   ├── OrdenInversionDto.cs
│   └── ActivoFinancieroDto.cs
├── Interfaces/           # Contratos e interfaces
├── Middleware/           # Middleware personalizado
├── Models/               # Entidades del dominio
├── Repositories/         # Patrón Repository
├── Services/             # Lógica de negocio
└── Program.cs           # Configuración de la aplicación
```

### Principios SOLID Implementados

1. **Single Responsibility**: Cada clase tiene una responsabilidad específica
2. **Open/Closed**: Extensible sin modificar código existente
3. **Liskov Substitution**: Interfaces intercambiables
4. **Interface Segregation**: Interfaces específicas y cohesivas
5. **Dependency Inversion**: Dependencias inyectadas mediante interfaces

### Guidelines de Desarrollo

- Usar DTOs para transferencia de datos
- Implementar validaciones en DTOs
- Mantener separación entre capas
- Seguir convenciones de nomenclatura de .NET



