param(
  [string]$OutputFile = "backend-structure.txt"
)

$ErrorActionPreference = "Stop"

$ProjectRoot = if ($PSScriptRoot) { $PSScriptRoot } else { (Get-Location).Path }
$ProjectRoot = [System.IO.Path]::GetFullPath($ProjectRoot)
$outputPath = Join-Path $ProjectRoot $OutputFile

$excludedDirectories = @(
  "node_modules",
  "dist",
  "dist-ssr",
  "bin",
  "obj",
  ".tmp",
  ".git",
  ".vs",
  ".idea",
  ".vscode",
  "TestResults",
  "coverage",
  "logs"
)

$excludedFileNames = @(
  "backend-code.txt",
  "backend-code-clean.txt",
  "backend-structure.txt",
  "backend-structure-clean.txt",
  "export-backend-code.ps1",
  "export-backend-structure.ps1"
)

function Get-RelativePath {
  param([string]$Path)

  $fullPath = [System.IO.Path]::GetFullPath($Path)
  $rootPath = $ProjectRoot.TrimEnd("\", "/")

  if ($fullPath.StartsWith($rootPath, [System.StringComparison]::OrdinalIgnoreCase)) {
    return $fullPath.Substring($rootPath.Length).TrimStart("\", "/")
  }

  return $fullPath
}

function Test-IsExcludedPath {
  param([string]$Path)

  $relativePath = (Get-RelativePath $Path).Replace("\", "/")
  $wrappedPath = "/$relativePath/"

  foreach ($directory in $excludedDirectories) {
    $normalizedDirectory = $directory.Trim("/", "\").Replace("\", "/")

    if ($wrappedPath -like "*/$normalizedDirectory/*") {
      return $true
    }
  }

  return $false
}

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$writer = [System.IO.StreamWriter]::new($outputPath, $false, $utf8NoBom)

try {
  $writer.WriteLine("============================================================")
  $writer.WriteLine("LOWCORTISOL BACKEND STRUCTURE EXPORT")
  $writer.WriteLine("Stack: ASP.NET Core + C#")
  $writer.WriteLine("Generated at: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
  $writer.WriteLine("Root: $ProjectRoot")
  $writer.WriteLine("Output: $OutputFile")
  $writer.WriteLine("============================================================")
  $writer.WriteLine("")

  Get-ChildItem -LiteralPath $ProjectRoot -Recurse -Force |
    Where-Object {
      $_.FullName -ne $outputPath -and
      -not (Test-IsExcludedPath $_.FullName) -and
      -not ($excludedFileNames -contains $_.Name)
    } |
    Sort-Object FullName |
    ForEach-Object {
      $relativePath = Get-RelativePath $_.FullName

      if ($_.PSIsContainer) {
        $writer.WriteLine("[DIR]  $relativePath")
      } else {
        $writer.WriteLine("[FILE] $relativePath")
      }
    }
}
finally {
  $writer.Dispose()
}

Write-Host "Backend structure exported to $outputPath"
