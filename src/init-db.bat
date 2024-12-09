@echo off
@echo =====================================================
@echo    Initializing Designly Database - Please Wait...
@echo =====================================================

:START
@echo.
set /p choice="Do you want to continue with the initialization? (Y/N): "
if /I "%choice%" NEQ "Y" goto CANCEL

:: Initialize Clients Database
@echo ---------------------------------------------
@echo    Initializing Clients Database...
@echo ---------------------------------------------
cd clients
cd clients.db
liquibase-init.cmd
if %ERRORLEVEL% NEQ 0 (
    @echo Error initializing Clients Database. Exiting...
    goto END
)
pause
cd ..\..
@echo.

:: Prompt to cancel before moving on
set /p cancel="Proceed to initialize Accounts Database? (Y/N): "
if /I "%cancel%" NEQ "Y" goto CANCEL

:: Initialize Accounts Database
@echo ---------------------------------------------
@echo    Initializing Accounts Database...
@echo ---------------------------------------------
cd accounts
cd accounts.db
liquibase-init.cmd
if %ERRORLEVEL% NEQ 0 (
    @echo Error initializing Accounts Database. Exiting...
    goto END
)
pause
cd ..\..
@echo.

:: Prompt to cancel before moving on
set /p cancel="Proceed to initialize Projects Database? (Y/N): "
if /I "%cancel%" NEQ "Y" goto CANCEL

:: Initialize Projects Database
@echo ---------------------------------------------
@echo    Initializing Projects Database...
@echo ---------------------------------------------
cd projects
cd projects.db
liquibase-init.cmd
if %ERRORLEVEL% NEQ 0 (
    @echo Error initializing Projects Database. Exiting...
    goto END
)
pause
cd ..\..
@echo.

@echo =====================================================
@echo    Designly Database Initialization Complete!
@echo =====================================================
goto END

:: Cancel Handling
:CANCEL
@echo.
@echo =====================================================
@echo    Initialization Canceled.
@echo =====================================================
goto END

:END
pause
