@echo off
REM "Author: Brylle Lloren"
REM "Date: 10/18/2015"

Title "BTMS Auto Backup Script"
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c-%%a-%%b)
For /f "tokens=1-2 delims=/:" %%a in ("%TIME%") do (set mytime=%%a%%b)


set infile="C:\BTMS\Release\GovEngine.sdf"
set outfile="E:\BTMS\Backup\GovEngine_%mydate%_%mytime%.sdf"
echo SRC: %infile%
echo DST: %outfile%
copy "%infile%" "%outfile%" /V /Y