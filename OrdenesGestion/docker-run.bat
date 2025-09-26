@echo off
echo Iniciando Gestion Ordenes con Docker Compose...

REM Verificar si Docker está ejecutándose
docker version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Docker no está ejecutándose. Por favor, inicia Docker Desktop.
    pause
    exit /b 1
)

REM Construir e iniciar los contenedores
docker-compose up --build -d

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Aplicación iniciada exitosamente!
    echo.
    echo 🌐 API disponible en: http://localhost:5111
    echo 🗄️  SQL Server en: localhost:1433
    echo 📖 Swagger UI: http://localhost:5111
    echo.
    echo Credenciales de SQL Server:
    echo   Usuario: sa
    echo   Contraseña: GestionOrdenes2024!
    echo.
    echo Comandos útiles:
    echo   Ver logs: docker-compose logs -f
    echo   Detener: docker-compose down
    echo   Reiniciar: docker-compose restart
    echo.
) else (
    echo ❌ Error al iniciar la aplicación
    exit /b 1
)

pause

