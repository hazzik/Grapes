for /r %%x in (src\Brandy.Grapes.\bin\Release\*.nupkg) do .nuget\NuGet.exe push "%%x" -ApiKey %1
for /r %%x in (src\Brandy.Grapes.NHibernate\bin\Release\*.nupkg) do .nuget\NuGet.exe push "%%x" -ApiKey %1
for /r %%x in (src\Brandy.Grapes.FluentNHibernate\bin\Release\*.nupkg) do .nuget\NuGet.exe push "%%x" -ApiKey %1

