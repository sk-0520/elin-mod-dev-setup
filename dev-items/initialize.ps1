param(
    [Parameter(Mandatory)][string] $ModName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectDirectoryPath = Join-Path -Path $PSScriptRoot -ChildPath ".."

# 不要ファイル破棄
$removeItems = @(
	(Join-Path -Path ".github" -ChildPath "copilot-instructions.md"),
	"docs",
	"Elin.Plugin.Generator.Test",
	"initialize.bat"
)
foreach ($item in $removeItems) {
	$itemPath = Join-Path -Path $projectDirectoryPath -ChildPath $item
	Remove-Item -Path $itemPath -Recurse -Force
}

# Mod 名をプロジェクトファイルに反映
$projectPropsPath = Join-Path -Path $projectDirectoryPath -ChildPath "Directory.Build.props.user"
[xml]$xml = Get-Content -LiteralPath $projectPropsPath -Raw -Encoding UTF8
$xml.Project.PropertyGroup.AssemblyName = $ModName
$xml.Save($projectPropsPath)
