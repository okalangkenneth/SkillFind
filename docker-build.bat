@echo off
set BUILDKIT_MAX_PARALLELISM=2
set DOCKER_BUILDKIT=1
"C:\Program Files\Docker\Docker\resources\bin\docker-compose.exe" -f "E:\Projects\inherited\SkillFind\docker-compose.yml" up -d --build > "E:\Projects\inherited\SkillFind\docker-build.log" 2> "E:\Projects\inherited\SkillFind\docker-build-err.log"
echo EXIT_CODE=%ERRORLEVEL% >> "E:\Projects\inherited\SkillFind\docker-build-err.log"
