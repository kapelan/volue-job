[System.Collections.ArrayList] $listOfInfrastructure = @("sql-env")

$listOfInfrastructure

$rootfolder = (Get-Location)

foreach ($env in $listOfInfrastructure) {
    write-host "Environment $($env)"

    $envfolder = "$rootfolder\$($env)"
    
	write-host "going to $($envfolder)"
	
    Set-Location $envfolder -Verbose

    if($Args[0] -eq "down") {
        invoke-expression 'docker-compose down'
    } else {
        invoke-expression 'docker-compose up -d --build'
    }
    
    Set-Location $rootfolder
}