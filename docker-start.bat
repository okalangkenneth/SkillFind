@echo off
set COMPOSE="C:\Program Files\Docker\Docker\resources\bin\docker-compose.exe"
set COMPOSE_FILE="E:\Projects\inherited\SkillFind\docker-compose.yml"

echo Starting all containers... > "E:\Projects\inherited\SkillFind\docker-start.log"
%COMPOSE% -f %COMPOSE_FILE% up -d >> "E:\Projects\inherited\SkillFind\docker-start.log" 2>&1
echo EXIT_CODE=%ERRORLEVEL% >> "E:\Projects\inherited\SkillFind\docker-start.log"
echo DONE >> "E:\Projects\inherited\SkillFind\docker-start.log"
