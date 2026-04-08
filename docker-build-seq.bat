@echo off
set BUILDKIT_MAX_PARALLELISM=2
set DOCKER_BUILDKIT=1
set COMPOSE="C:\Program Files\Docker\Docker\resources\bin\docker-compose.exe"
set COMPOSE_FILE="E:\Projects\inherited\SkillFind\docker-compose.yml"
set LOG="E:\Projects\inherited\SkillFind\docker-build.log"
set ERR="E:\Projects\inherited\SkillFind\docker-build-err.log"

echo. > %LOG%
echo. > %ERR%

echo [1/6] Building apigateway... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build apigateway >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: apigateway >> %ERR% & goto :done )

echo [2/6] Building notification-service... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build notification-service >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: notification-service >> %ERR% & goto :done )

echo [3/6] Building search-service... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build search-service >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: search-service >> %ERR% & goto :done )

echo [4/6] Building jobcategory-api... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build jobcategory-api >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: jobcategory-api >> %ERR% & goto :done )

echo [5/6] Building jobseeker-api... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build jobseeker-api >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: jobseeker-api >> %ERR% & goto :done )

echo [6/6] Building jobposting-api... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% build jobposting-api >> %LOG% 2>> %ERR%
if %ERRORLEVEL% neq 0 ( echo FAILED: jobposting-api >> %ERR% & goto :done )

echo All images built. Starting containers... >> %ERR%
%COMPOSE% -f %COMPOSE_FILE% up -d >> %LOG% 2>> %ERR%
echo EXIT_CODE=%ERRORLEVEL% >> %ERR%

:done
echo BUILD COMPLETE >> %ERR%
