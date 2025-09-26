@echo off
echo Verificando instalación de Docker...
echo.

REM Verificar Docker
docker --version
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Docker no está instalado o no está en el PATH
    echo.
    echo Por favor:
    echo 1. Instala Docker Desktop desde: https://www.docker.com/products/docker-desktop/
    echo 2. Reinicia tu computadora
    echo 3. Abre Docker Desktop y espera a que inicie
    echo 4. Ejecuta este script nuevamente
    pause
    exit /b 1
)

echo ✅ Docker está instalado!
echo.

REM Verificar Docker Compose
docker-compose --version
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Docker Compose no está disponible
    pause
    exit /b 1
)

echo ✅ Docker Compose está disponible!
echo.

REM Verificar que Docker esté ejecutándose
docker ps >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Docker no está ejecutándose
    echo.
    echo Por favor:
    echo 1. Abre Docker Desktop
    echo 2. Espera a que aparezca "Docker Desktop is running"
    echo 3. Ejecuta este script nuevamente
    pause
    exit /b 1
)

echo ✅ Docker está ejecutándose correctamente!
echo.
echo 🚀 ¡Todo listo para ejecutar la aplicación!
echo.
echo Para iniciar la aplicación ejecuta:
echo   docker-run.bat
echo.
pause

