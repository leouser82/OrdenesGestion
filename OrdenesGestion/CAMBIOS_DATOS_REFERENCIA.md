# Cambios para Datos de Referencia desde Base de Datos

## Resumen de Cambios

Se han realizado los siguientes cambios para que los estados de orden, tipos de activo y activos financieros se lean desde la base de datos en lugar de estar hardcodeados, eliminando las operaciones CRUD para estos datos de referencia.

## Archivos Creados

### 1. `Interfaces/IReferenceDataRepository.cs`
- Nueva interfaz para acceso de solo lectura a datos de referencia
- Métodos para obtener estados de orden, tipos de activo y activos financieros
- No incluye operaciones de creación, actualización o eliminación

### 2. `Repositories/ReferenceDataRepository.cs`
- Implementación del repositorio de datos de referencia
- Usa `AsNoTracking()` para consultas de solo lectura optimizadas
- Incluye navegación a entidades relacionadas donde es necesario

### 3. `Controllers/ReferenceDataController.cs`
- Nuevo controlador para exponer endpoints de consulta de datos de referencia
- Endpoints disponibles:
  - `GET /api/referencedata/estados-orden` - Todos los estados
  - `GET /api/referencedata/estados-orden/{id}` - Estado específico
  - `GET /api/referencedata/tipos-activo` - Todos los tipos de activo
  - `GET /api/referencedata/tipos-activo/{id}` - Tipo de activo específico
  - `GET /api/referencedata/activos-financieros` - Todos los activos financieros
  - `GET /api/referencedata/activos-financieros/{id}` - Activo financiero específico
  - `GET /api/referencedata/activos-financieros/por-tipo/{tipoActivoId}` - Activos por tipo

## Archivos Modificados

### 1. `Services/OrdenInversionService.cs`
- **Cambio de dependencia**: Ahora usa `IReferenceDataRepository` en lugar de `IRepository<EstadoOrden>`
- **Validaciones mejoradas**: Usa el nuevo repositorio para validar estados y obtener información de activos
- **Mantiene funcionalidad**: Todas las operaciones de órdenes siguen funcionando igual

### 2. `Program.cs`
- **Registro de dependencias**: Se agregó `IReferenceDataRepository` al contenedor de DI
- **Eliminación**: Se removió el registro de `IActivoFinancieroService` (ya no utilizado)

### 3. `GestionOrdenes.http`
- **Endpoints actualizados**: Se agregaron ejemplos de uso para todos los nuevos endpoints
- **Documentación completa**: Incluye ejemplos de autenticación y uso de tokens JWT

## Datos de Referencia

Los siguientes datos están configurados como semilla en la base de datos:

### Estados de Orden
1. En proceso
2. Ejecutada  
3. Cancelada

### Tipos de Activo
1. Acción (Comisión: 0.6%, Impuesto: 21%)
2. Bono (Comisión: 0.2%, Impuesto: 21%)
3. FCI (Sin comisión ni impuesto)

### Activos Financieros
- **Acciones**: AAPL, GOOGL, MSFT, KO, WMT
- **Bonos**: AL30, GD30
- **FCI**: Delta Pesos, Fima Premium

## Características Técnicas

### Optimizaciones
- Uso de `AsNoTracking()` para consultas de solo lectura
- Consultas optimizadas con navegación incluida donde es necesario
- Repositorio especializado para datos de referencia

### Seguridad
- Todos los endpoints requieren autenticación JWT
- Mantiene la estructura de autorización existente

### Escalabilidad
- Separación clara entre datos de referencia y datos transaccionales
- Fácil mantenimiento de datos maestros desde la base de datos
- No hay operaciones CRUD expuestas para datos de referencia

## Uso

Para obtener datos de referencia para crear una orden:

1. **Obtener tipos de activo disponibles**:
   ```
   GET /api/referencedata/tipos-activo
   ```

2. **Obtener activos de un tipo específico**:
   ```
   GET /api/referencedata/activos-financieros/por-tipo/1
   ```

3. **Crear orden con los IDs obtenidos**:
   ```
   POST /api/ordenesinversion
   {
     "cuentaId": 12345,
     "activoId": 1,
     "cantidad": 10,
     "precio": 177.97,
     "operacion": "C"
   }
   ```

## Ventajas del Nuevo Enfoque

1. **Datos centralizados**: Todos los datos de referencia están en la base de datos
2. **Mantenimiento simplificado**: No hay código hardcodeado para datos maestros
3. **Flexibilidad**: Fácil agregar nuevos activos, estados o tipos sin cambios de código
4. **Rendimiento**: Consultas optimizadas para datos de solo lectura
5. **Separación de responsabilidades**: Clara distinción entre datos maestros y transaccionales


