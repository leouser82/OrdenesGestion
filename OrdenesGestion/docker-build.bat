@echo off
echo Construyendo la imagen Docker de Gestion Ordenes...

REM Construir la imagen
docker build -t gestion-ordenes-api:latest .

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Imagen construida exitosamente!
    echo.
    echo Para ejecutar la aplicación completa:
    echo   docker-compose up -d
    echo.
    echo Para ver los logs:
    echo   docker-compose logs -f
    echo.
    echo Para detener:
    echo   docker-compose down
) else (
    echo.
    echo ❌ Error al construir la imagen
    exit /b 1
)

pause

