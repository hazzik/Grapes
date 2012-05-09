@echo off
set psake=packages\psake.4.1.0\tools\psake

pushd .nuget
echo Restoring nuget packages
call nuget install packages.config -o ..\packages
popd

call %psake% %%*

pause