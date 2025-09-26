# Gestión de Órdenes API

API REST para la gestión de órdenes de inversión en activos financieros, desarrollada con .NET 8 siguiendo principios SOLID y arquitectura simplificada.

## Características

- ✅ **Principios SOLID**: Arquitectura limpia con separación de responsabilidades
- ✅ **Entity Framework Core**: ORM para acceso a datos con SQL Server
- ✅ **Repository Pattern**: Abstracción de acceso a datos con auto-save
- ✅ **Arquitectura Simplificada**: Sin Unit of Work para mayor simplicidad
- ✅ **JWT Authentication**: Seguridad basada en tokens
- ✅ **Swagger/OpenAPI**: Documentación interactiva de la API
- ✅ **Middleware personalizado**: Manejo global de excepciones
- ✅ **CORS**: Soporte para aplicaciones web

## Tecnologías

- .NET 8
- Entity Framework Core 8
- SQL Server LocalDB
- JWT Bearer Authentication
- Swagger/OpenAPI
- Mapeo manual en servicios (más control y simplicidad)

## Estructura del Proyecto

```
GestionOrdenes/
├── Controllers/           # Controladores de la API
├── Data/                 # DbContext y configuración de EF
├── DTOs/                 # Data Transfer Objects
├── Interfaces/           # Interfaces y contratos
├── Middleware/           # Middleware personalizado
├── Models/               # Entidades del dominio
├── Repositories/         # Implementación del patrón Repository
├── Services/             # Lógica de negocio
└── Program.cs           # Configuración de la aplicación
```

## Base de Datos

La API utiliza las siguientes tablas:

- **TipoActivo**: Tipos de activos financieros (Acción, Bono, FCI)
- **EstadoOrden**: Estados de las órdenes (En proceso, Ejecutada, Cancelada)
- **ActivoFinanciero**: Activos financieros disponibles
- **OrdenInversion**: Órdenes de inversión realizadas

## Instalación y Configuración

### Prerrequisitos

- .NET 8 SDK
- SQL Server LocalDB (incluido con Visual Studio)

### Pasos para ejecutar

1. **Clonar o descargar el proyecto**

2. **Restaurar paquetes NuGet**
   ```bash
   dotnet restore
   ```

3. **Configurar la base de datos**
   
   La cadena de conexión ya está configurada en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GestionOrdenes;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
   }
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a Swagger UI**
   
   Abrir en el navegador: `https://localhost:7xxx` (el puerto se mostrará en la consola)

## Autenticación

La API utiliza JWT Bearer Authentication. Para acceder a los endpoints protegidos:

### Usuarios de prueba

- **Admin**: `username: admin`, `password: admin123`
- **User**: `username: user`, `password: user123`

### Proceso de autenticación

1. **Login**: `POST /api/auth/login`
   ```json
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Respuesta**:
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
     "expiration": "2024-01-01T00:00:00Z",
     "username": "admin",
     "role": "Admin"
   }
   ```

3. **Usar el token**: Agregar en el header `Authorization: Bearer {token}`

## Endpoints Principales

### Autenticación
- `POST /api/auth/login` - Autenticar usuario

### Órdenes de Inversión
- `GET /api/ordenesinversion` - Obtener todas las órdenes
- `GET /api/ordenesinversion/{id}` - Obtener orden por ID
- `GET /api/ordenesinversion/cuenta/{cuentaId}` - Órdenes por cuenta
- `GET /api/ordenesinversion/activo/{activoId}` - Órdenes por activo
- `GET /api/ordenesinversion/estado/{estadoId}` - Órdenes por estado
- `POST /api/ordenesinversion` - Crear nueva orden
- `PUT /api/ordenesinversion/{id}/estado` - Actualizar estado de orden
- `DELETE /api/ordenesinversion/{id}` - Eliminar orden (solo Admin)

### Activos Financieros
- `GET /api/activosfinancieros` - Obtener todos los activos
- `GET /api/activosfinancieros/{id}` - Obtener activo por ID
- `GET /api/activosfinancieros/ticker/{ticker}` - Obtener activo por ticker
- `GET /api/activosfinancieros/tipo/{tipoId}` - Activos por tipo
- `PUT /api/activosfinancieros/{id}/precio` - Actualizar precio (solo Admin)

## Ejemplos de Uso

### Crear una orden de inversión

```json
POST /api/ordenesinversion
{
  "cuentaId": 1,
  "activoId": 1,
  "cantidad": 10,
  "precio": 177.97,
  "operacion": "C"
}
```

### Actualizar estado de orden

```json
PUT /api/ordenesinversion/1/estado
{
  "estadoId": 2
}
```

## Principios SOLID Implementados

1. **Single Responsibility Principle**: Cada clase tiene una sola responsabilidad
2. **Open/Closed Principle**: Extensible sin modificar código existente
3. **Liskov Substitution Principle**: Las interfaces pueden ser sustituidas por sus implementaciones
4. **Interface Segregation Principle**: Interfaces específicas y cohesivas
5. **Dependency Inversion Principle**: Dependencias inyectadas mediante interfaces

## Características de Seguridad

- Autenticación JWT
- Autorización basada en roles
- Validación de modelos
- Manejo seguro de excepciones
- CORS configurado

## Manejo de Errores

El middleware personalizado maneja automáticamente:
- Argumentos inválidos (400 Bad Request)
- Recursos no encontrados (404 Not Found)
- Acceso no autorizado (401 Unauthorized)
- Errores internos del servidor (500 Internal Server Error)

## Desarrollo

Para continuar el desarrollo:

1. Los modelos están mapeados con Entity Framework
2. Los repositorios implementan el patrón Repository
3. Los servicios contienen la lógica de negocio
4. Los controladores manejan las peticiones HTTP
5. El middleware maneja excepciones globalmente

## Notas Importantes

- La base de datos se crea automáticamente al ejecutar la aplicación
- Los datos de prueba se insertan automáticamente
- Los usuarios de prueba están hardcodeados (en producción usar base de datos)
- El token JWT expira en 24 horas
- Swagger UI está disponible en la raíz de la aplicación
