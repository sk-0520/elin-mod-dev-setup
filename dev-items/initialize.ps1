param(
    [Parameter(Mandatory)][string] $ModName
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectDirectoryPath = Join-Path -Path $PSScriptRoot -ChildPath ".."

# 不要ファイル破棄
$removeItems = @(
	(Join-Path -Path ".github" -ChildPath "copilot-instructions.md"),
	(Join-Path -Path ".github" -ChildPath | Join-Path -ChildPath "workflows" | Join-Path -ChildPath "mod-template.yaml"),
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
[xml]$propsXml = Get-Content -LiteralPath $projectPropsPath -Raw -Encoding UTF8
$propsXml.Project.PropertyGroup.AssemblyName = $ModName
$propsXml.Save($projectPropsPath)

# ソリューションから削除プロジェクトの破棄
$solutionPath = Join-Path -Path $projectDirectoryPath -ChildPath "Elin.Plugin.slnx"
[xml]$solutionXml = Get-Content -LiteralPath $solutionPath -Raw -Encoding UTF8
$solutionXml.SelectNodes("//*/Project") | ForEach-Object {
	if ($_.Path -eq "Elin.Plugin.Generator.Test/Elin.Plugin.Generator.Test.csproj") {
		$_.ParentNode.RemoveChild($_) | Out-Null
	}
}
$solutionXml.Save($solutionPath)
