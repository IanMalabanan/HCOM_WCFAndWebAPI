# To Update Project Name



#$ProjectName="ProjAlpha"
$TemplateName="CTI.Template"
$ProjectName=$args[0]

$Exclude = @("packages", "node_modules")


$files = Get-ChildItem  *.* -rec  -Exclude *.dll,*.exe, *.targets,SetupScript.ps1,*.svn-base,*.svn*,*.vs*
foreach ($fileName in $files) {


#ECHO $fileName
(get-content $fileName ) | foreach-object {$_ -replace $TemplateName, $ProjectName} | set-content $fileName 
}

Get-ChildItem -recurse -name | ForEach-Object { Move-Item $_ $_.replace($TemplateName, $ProjectName) | Where {($_.PSIsContainer) -and (!$Exclude.Contains($_.Name))}}
  