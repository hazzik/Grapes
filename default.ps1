Framework '4.0'

properties {
	$configuration = 'Debug'
	$platform = 'Any CPU'
	$fluentmigrator = Get-Item "packages/FluentMigrator.Tools.*/tools/AnyCPU/40/Migrate.exe"
	$xunitconsole = Get-Item "packages/xunit.runners.*/tools/xunit.console.clr4.exe"
}

Task Default -Depends Build

Task Build {
	MSBuild	-t:Build -p:Configuration=$configuration
}

Task Test -Depends Build {
	$testsAssembly = Get-Item "src/*/bin/$configuration/*Tests.dll"
	$testsAssembly | ForEach-Object { Invoke-Expression "$xunitconsole $_" }
}

Task MigrateUp -Depends Build {
	$migrationsAssembly = Get-Item "src/Migrations/bin/$configuration/*Migrations.dll"
	$migrationsAssembly | ForEach-Object { Invoke-Expression "$fluentmigrator --provider=sqlserver --connectionString=Main --target=$_" }
}
